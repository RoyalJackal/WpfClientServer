using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Dto;
using Client.Services;

namespace Client.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private string _username;
    private string _companyName;

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

    public string CompanyName
    {
        get => _companyName;
        set
        {
            if (value == _companyName) return;
            _companyName = value;
            OnPropertyChanged();
        }
    }
    
    private const string UserInfoPath = "/info";

    public async Task<bool> UpdateUserInfoAsync()
    {
        var response = await HttpService.GetAsync(UserInfoPath, true);
        if (!response.IsSuccessStatusCode)
        {
            Username = "";
            CompanyName = "";
            return false;
        }

        var dto = await JsonSerializer.DeserializeAsync<UserInfoDto>(
            await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions{PropertyNameCaseInsensitive = true});

        Username = dto.Username;
        CompanyName = dto.CompanyName;
        return true;
    }

    public void SignOut() =>
        HttpService.ClearToken();

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}