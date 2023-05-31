using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Dto;
using Client.Services;

namespace Client.ViewModels;

public class SignUpViewModel : INotifyPropertyChanged
{
    private ObservableCollection<CompanyDto?> _companies;
    private CompanyDto? _selectedCompany;
    private string _username;
    private string _email;
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
    
    public string Email
    {
        get => _email;
        set
        {
            if (value == _email) return;
            _email = value;
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
    private const string SignUpPath = "/Auth/register";
    
    public async Task LoadCompaniesAsync()
    {
        var response = await HttpService.GetAsync(CompaniesPath);
        if (!response.IsSuccessStatusCode)
            throw new Exception(response.ReasonPhrase);

        Companies = await JsonSerializer.DeserializeAsync<ObservableCollection<CompanyDto>>(
            await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
    }

    public async Task<bool> SignUpAsync()
    {
        if (Password == "" || Username == "" || Email == "" || SelectedCompany == null)
        {
            Error = "All fields must be filled.";
            return false;
        }
            
        var dto =  new SignUpDto(Username, Email, Password, SelectedCompany.Id);

        var response = await HttpService.PostAsync(SignUpPath, dto);

        if (!response.IsSuccessStatusCode)
        {
            var responseText = await response.Content.ReadAsStringAsync();
            Error = responseText == "" ? "Unknown error." : responseText;
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