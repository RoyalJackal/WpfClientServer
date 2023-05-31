using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Dto;
using Client.Services;

namespace Client.ViewModels;

public class SignInViewModel : INotifyPropertyChanged
{
    private ObservableCollection<CompanyDto?> _companies;
    private CompanyDto? _selectedCompany;
    private ObservableCollection<string> _usernames;
    private string? _selectedUsername;
    private string _username;
    private string _password;
    private string _error;

    public ObservableCollection<CompanyDto?> Companies
    {
        get => _companies;
        set
        {
            if (Equals(value, _companies)) return;
            _companies = value;
            OnPropertyChanged();
        }
    }

    public CompanyDto? SelectedCompany
    {
        get => _selectedCompany;
        set
        {
            if (Equals(value, _selectedCompany)) return;
            _selectedCompany = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> Usernames
    {
        get => _usernames;
        set
        {
            if (Equals(value, _usernames)) return;
            _usernames = value;
            OnPropertyChanged();
        }
    }

    public string? SelectedUsername
    {
        get => _selectedUsername;
        set
        {
            if (value == _selectedUsername) return;
            _selectedUsername = value;
            OnPropertyChanged();
            OnPropertyChanged();
        }
    }

    public string Username
    {
        get => _username;
        set
        {
            if (value == _username) return;
            _username = value;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (value == _password) return;
            _password = value;
            OnPropertyChanged();
        }
    }

    public string Error
    {
        get => _error;
        set
        {
            if (value == _error) return;
            _error = value;
            OnPropertyChanged();
        }
    }

    private const string CompaniesPath = "/Company";
    private const string SignInPath = "/Auth/login";

    
    public async Task LoadCompaniesAsync()
    {
        var response = await HttpService.GetAsync(CompaniesPath);
        if (!response.IsSuccessStatusCode)
            throw new Exception(response.ReasonPhrase);

        Companies = await JsonSerializer.DeserializeAsync<ObservableCollection<CompanyDto>>(
            await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
    }

    public async Task LoadCompanyUsersAsync()
    {
        if (SelectedCompany == null)
            return;
        var response = await HttpService.GetAsync(CompaniesPath + $"/{SelectedCompany.Id}");
        if (!response.IsSuccessStatusCode)
            throw new Exception(response.ReasonPhrase);

        Usernames = await JsonSerializer.DeserializeAsync<ObservableCollection<string>>(
            await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
    }

    public void Clear()
    {
        SelectedCompany = null;
        SelectedUsername = null;
        Username = "";
        Password = "";
        Error = "";
    }
    
    public async Task<bool> SignInAsync(bool isCompany)
    {
        if (Password == "" || (Username == "" && SelectedUsername == null))
        {
            Error = "All fields must be filled.";
            return false;
        }
            
        var dto = isCompany ? new SignInDto(SelectedUsername, Password) : new SignInDto(Username, Password);

        var response = await HttpService.PostAsync(SignInPath, dto);
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            Error = "Wrong username or password.";
            return false;
        }

        if (!response.IsSuccessStatusCode)
        {
            Error = "Unknown error.";
            return false;
        }

        HttpService.SetToken((await JsonSerializer.DeserializeAsync<TokenDto>(
            await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions{PropertyNameCaseInsensitive = true}))?.Token);
        return true;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}