using System;
using System.Collections.Generic;
using System.IO;
using CodeTitans.JSon;
using Ucss;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VoxelBusters.NativePlugins;

public class RegPanel : MonoBehaviour
{
    public InputField Login;
    public InputField Pass;
    public InputField Pass2;
    public InputField EmailText;
    public InputField PhoneText;
    public Text BdateText;
    public Text CountryText;
    public InputField FioText;
    public Text KodText;
    public Text MoneyText;
    public RegErrorField ELogin;
    public RegErrorField EPass;
    public RegErrorField EPass2;
    public RegErrorField EEmailText;
    public RegErrorField EPhoneText;
    public RegErrorField EBdateText;
    public RegErrorField ECountryText;
    public RegErrorField EFioText;
    public RegErrorField EKodText;
    public RegErrorField EMoneyText;
    public RegErrorField ETotalText;
    public RegErrorField EAvatarText;
    public RegErrorField FileNameText;
    public bool Ready = false;
    public RawImage CapchaImage;
    public RawImage AvatarImage;
  //  private FileInfo _avatarInfo;
    public ToggleGroup MoneyGroup;
    public Toggle RealToggle;
    public Toggle VirtualToggle;
    public Texture2D AvatarImg;
    public DropDownPanel Date;
    public DropDownPanel Month;
    public DropDownPanel Year;
    public DropDownPanel Country;
    public RegErrorMsg ErrorMsg;
    public DateSelector DateSelector;
    public CountrySelector CountrySelector;
    public string MoneyTip = "2";
    public string[] CountryList = new[]
    {"Afghanistan","Aland Islands","Albania","Algeria","American Samoa","Andorra","Angola","Anguilla","Antarctica",
    "Antigua and Barbuda","Argentina","Armenia","Aruba","Australia","Austria","Azerbaijan","Bahamas","Bahrain",
    "Bangladesh","Barbados","Belarus","Belgium","Belize","Benin","Bermuda","Bhutan","Bolivia","Bonaire",
    "Bosnia and Herzegovina","Botswana","Bouvet Island","Brazil","British Indian Ocean Territory","Brunei Darussalam",
    "Bulgaria","Burkina Faso","Burundi","Cambodia","Cameroon","Canada","Cape Verde","Cayman Islands",
    "Central African Republic","Chad","Chile","China","Christmas Island","Cocos Keeling Islands","Colombia","Comoros",
    "Congo","Congo,The Democratic Republic of the","Cook Islands","Costa Rica","Cote d'Ivoire","Croatia","Cuba",
    "Curacao","Cyprus","Czech Republic","Denmark","Djibouti","Dominica","Dominican Republic","Ecuador","Egypt",
    "El Salvador","Equatorial Guinea","Eritrea","Estonia","Ethiopia","Falkland Islands Malvinas","Faroe Islands","Fiji",
    "Finland","France","French Guiana","French Polynesia","French Southern Territories","Gabon","Gambia","Georgia",
    "Germany","Ghana","Gibraltar","Greece","Greenland","Grenada","Guadeloupe","Guam","Guatemala","Guernsey","Guinea",
    "Guinea-Bissau","Guyana","Haiti","Heard Island and McDonald Islands","Holy See Vatican City State","Honduras",
    "Hong Kong","Hungary","Iceland","India","Indonesia","Iran","Iraq","Ireland","Isle of Man","Israel","Italy","Jamaica"
    ,"Japan","Jersey","Jordan","Kazakhstan","Kenya","Kiribati","Korea Democratic People's Republic","Korea Republic",
    "Kuwait","Kyrgyzstan","Lao People's Democratic Republic","Latvia","Lebanon","Lesotho","Liberia",
    "Libyan Arab Jamahiriya","Liechtenstein","Lithuania","Luxembourg","Macao","Macedonia","Madagascar","Malawi",
    "Malaysia","Maldives","Mali","Malta","Marshall Islands","Martinique","Mauritania","Mauritius","Mayotte","Mexico",
    "Micronesia","Moldova","Monaco","Mongolia","Montenegro","Montserrat","Morocco","Mozambique","Myanmar","Namibia",
    "Nauru","Nepal","Netherlands","New Caledonia","New Zealand","Nicaragua","Niger","Nigeria","Niue","Norfolk Island",
    "Northern Mariana Islands","Norway","Oman","Pakistan","Palau","Palestinian Territory,Occupied","Panama",
    "Papua New Guinea","Paraguay","Peru","Philippines","Pitcairn","Poland","Portugal","Puerto Rico","Qatar","Reunion",
    "Romania","Russian Federation","Rwanda","Saint Barthelemy","Saint Helena,Ascension and Tristan Da Cunha",
    "Saint Kitts and Nevis","Saint Lucia","Saint Martin French Part","Saint Pierre and Miquelon",
    "Saint Vincent and The Grenadines","Samoa","San Marino","Sao Tome and Principe","Saudi Arabia","Senegal","Serbia",
    "Seychelles","Sierra Leone","Singapore","Sint Maarten Dutch Part","Slovakia","Slovenia","Solomon Islands","Somalia",
    "South Africa","South Georgia and The South Sandwich Islands","South Sudan","Spain","Sri Lanka","Sudan","Suriname",
    "Svalbard and Jan Mayen","Swaziland","Sweden","Switzerland","Syrian Arab Republic","Taiwan,Province of China",
    "Tajikistan","Tanzania","Thailand","Timor-Leste","Togo","Tokelau","Tonga","Trinidad and Tobago","Tunisia","Turkey",
    "Turkmenistan","Turks and Caicos Islands","Tuvalu","Uganda","Ukraine","United Arab Emirates","United Kingdom",
    "United States","United States Minor Outlying Islands","Uruguay","Uzbekistan","Vanuatu","Venezuela","Viet Nam",
    "Virgin Islands,British","Virgin Islands,U.S.","Wallis and Futuna","Western Sahara","Yemen","Zambia","Zimbabwe"
};
    public void Awake()
    {
        HideErrorMsg();
        _canvasGroup = GetComponent<CanvasGroup>();
        if (CapchaImage != null)
            GetCapcha(Net.ServerURL + "/api/default/captcha");
        var n = System.DateTime.Now;
        BdateText.text = (n.Year - 18) + "-1-1";

    }
    public GUISkin[] skins;
    public Texture2D file, folder, back, drive;

    //initialize file browser
  //  FileBrowser fb = new FileBrowser();
    //private bool ShowFileBrowser = false;
    private CanvasGroup _canvasGroup;

    public void OnUpdateCaptcha()
    {
        GetCapcha(Net.ServerURL + "/api/default/captcha");
    }

    public void HideErrorMsg()
    {
        ErrorMsg.HideError();
    }

    public void OnPass2Changed()
    {
        Pass2Changed();
    }
    public void OnFioChanged()
    {
        FioChanged();
    }
    public void OnPhoneChanged()
    {
        PhoneChanged();
    }
    public void OnPassChanged()
    {
        PassChanged();
    }
    public void OnEmailChanged()
    {
        EmailChanged();
    }
    public void OnLoginChanged()
    {
        LoginChanged();

    }
    public bool Pass2Changed(bool falg = false)
    {
        if (Pass2.text != Pass.text)
        {
            ShowError("pass2", GameController.LangManager.GetTextValue("Reg.Pass2Error"));
            return false;
        }
        else
        {
            ShowError("pass2", "");

            return true;
        }
    }
    public bool FioChanged()
    {
        foreach (char c in FioText.text)
        {
            if (!Char.IsLetter(c) && c != ' ')
            {
                ShowError("fio", GameController.LangManager.GetTextValue("Reg.FioError"));
                return false;
            }
        }
        if (FioText.text.Length > 36 || FioText.text.Length < 4)
        {
            ShowError("fio", GameController.LangManager.GetTextValue("Reg.FioError"));
            return false;
        }
        ShowError("fio", "");
        return true;
    }
    public bool PhoneChanged()
    {
        if (PhoneText.text.Length == 0)
        {
            ShowError("phone", GameController.LangManager.GetTextValue("Reg.PhoneError"));
            return false;
        }
        foreach (char c in PhoneText.text)
        {
            if (!Char.IsDigit(c))
            {
                ShowError("phone", GameController.LangManager.GetTextValue("Reg.PhoneError"));
                return false;
            }
        }
        ShowError("phone", "");
        return true;
    }
    public bool PassChanged(bool falg = false)
    {
        if (Pass.text.Length == 0 && !falg)
            return false;
        bool haveNumber = false;
        bool haveUpper = false;
        bool haveSimbol = false;
        bool flag = false;
        foreach (char c in Pass.text)
        {
            if (Char.IsUpper(c))
            {
                haveUpper = true;
            }

            if (Char.IsDigit(c))
            {
                haveNumber = true;
            }

            if (!Char.IsSymbol(c))
            {
                flag = true;
            }
        }
        print(flag + ":" + haveUpper + ":" + haveNumber);
        if (!flag || !haveUpper || !haveNumber)
        {
            ShowError("pass", GameController.LangManager.GetTextValue("Reg.PassError1"));
            return false;
        }

        if ((Pass.text.Length > 36 || Pass.text.Length < 4))
        {
            ShowError("pass", GameController.LangManager.GetTextValue("Reg.PassError"));
            return false;
        }
        else
        {
            ShowError("pass", "");
            return true;

        }
    }
 
    public bool EmailChanged()
    {
        foreach (char c in EmailText.text)
        {
            if (c == '@')
            {
                ShowError("email", "");
                return true;
            }
        }

        ShowError("email", GameController.LangManager.GetTextValue("Reg.MailError"));
        return false;
    }
    public bool LoginChanged()
    {
        foreach (char c in Login.text)
        {
            if (!Char.IsLetterOrDigit(c))
            {
                ShowError("login", GameController.LangManager.GetTextValue("Reg.LoginError"));
                return false;
            }
        }
        if (Login.text.Length > 36 || Login.text.Length < 4)
        {
            ShowError("login", GameController.LangManager.GetTextValue("Reg.LoginError"));
            return false;
        }
        else
        {
            ShowError("login", "");
            SendCheckLogin(Login.text);
            return true;
        }
    }
    public void OnPickImageFromAlbum()
    {
        if (_canvasGroup != null) _canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;
        // Set popover to last touch position
        NPBinding.UI.SetPopoverPointAtLastTouchPosition();

        // Pick image
        NPBinding.MediaLibrary.PickImage(eImageSource.ALBUM, 1.0f, PickImageFinished);
    }
    private void PickImageFinished(ePickImageFinishReason _reason, Texture2D _image)
    {
        if (_canvasGroup != null) _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
        if (_image != null)
            AvatarImg = _image;
        AvatarImage.texture = _image;
        //AddNewResult("Request to pick image from gallery finished. Reason for finish is " + _reason + ".");
        //AppendResult(string.Format("Selected image is {0}.", (_image == null ? "NULL" : _image.ToString())));
        //m_texture = _image;
    }
    public bool OnAvatarChanged(Texture2D av)
    {
        if (av != null)
        {

            //if (fb.outputFile.Length > 350000)
            //{
            //    ShowError("avatar", GameController.LangManager.GetTextValue("Reg.AvatarError"));
            //    return false;
            //}
            //else
            //{
            ShowError("avatar", "");
            return true;
            //}
        }
        else
        {
            return true;
        }
    }
    public bool CheckAllFields()
    {
        print("CheckAllFields " + MoneyTip);
        if (MoneyTip == "1")
        {
            if (LoginChanged() && EmailChanged() && PassChanged(true) && PhoneChanged() && FioChanged() &&
                Pass2Changed(true) && OnAvatarChanged(AvatarImg))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (LoginChanged() && EmailChanged() && PassChanged() && Pass2Changed())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void OnClose()
    {
        GUIController.Instance.ShowLoginPanel();
    }
  
    /// <summary>
    /// играем на виртуал
    /// </summary>
    public virtual void OnVirt()
    {

        MoneyTip = "2";
    }

    public void OnClick()
    {
        ErrorMsg.HideError();
    }
    public void OnDateSelect()
    {
        DateSelector.Init();
        DateSelector.TargetField = BdateText;
        DateSelector.gameObject.transform.localPosition = Vector3.zero;
        DateSelector.gameObject.SetActive(true);
        //DateSelector.Show();
    }
    public void OnCountrySelect()
    {
        CountrySelector.Init();
        CountrySelector.TargetField = CountryText;
        CountrySelector.gameObject.transform.localPosition = Vector3.zero;
        CountrySelector.gameObject.SetActive(true);
        //DateSelector.Show();
    }
    /// <summary>
    /// играем на реал
    /// </summary>
    public virtual void OnReal()
    {
        MoneyTip = "1";
    }
    public void OnReg()
    {
        //print("_moneyTip " + _moneyTip);
        HideErrorMsg();
        //провяем валидность ввода
        if (CheckAllFields())
        {
            if (_canvasGroup != null) _canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;
            WWWForm data = new WWWForm();
            data.AddField("username", Login.text);
            data.AddField("password_hash", Pass.text);
            data.AddField("email", EmailText.text);
            data.AddField("phone", PhoneText.text);
            data.AddField("birthday", BdateText.text);
            //   data.AddField("birthday", Year.LabelText.text + "-" + (Month.ActiveItemID + 1) + "-" + Date.LabelText.text);
            print("BdateText.text " + BdateText.text);
            data.AddField("country_id", CountrySelector.SelectedID.ToString());
            data.AddField("full_name", FioText.text);
            data.AddField("money_cat_id", MoneyTip);
            data.AddField("gender", "man");
            data.AddField("captcha", KodText.text);
            if (AvatarImg != null)
            {
                var bytes = AvatarImg.EncodeToPNG();
                data.AddBinaryData("avatar", bytes, "avatar.png", "image/png");
            }
            UCSS.HTTP.PostForm(Net.ServerURL + "/api/users?access-token=" + GameController.Token, data,
                new EventHandlerHTTPString(OnRegAnsw), new EventHandlerServiceError(Net.Instance.OnHTTPError),
                new EventHandlerServiceTimeOut(Net.Instance.OnHTTPTimeOut), 10);
        }
        else
        {
            ShowError("avatar", GameController.LangManager.GetTextValue("Reg.TotalError"));
        }

    }
    public void OnRegAnsw(string text, string transactionid)
    {
        IJSonMutableObject obj = Net.ReadPack(text);
        print(text);
        if (_canvasGroup != null) _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
        if (obj != null)
        {
            if (obj.Contains("status"))
            {

                switch (obj["status"].StringValue)
                {
                    case "created":
                        Net.Auth(Login.text, Pass.text);
                        print("OK!");
                        ErrorMsg.HideError();
                        break;
                    case "CAPTCHA_IS_NOT_VALID":
                        EKodText.SetError(GameController.LangManager.GetTextValue("Reg.WrongCaptcha"));
                        break;
                    case "UNSUPPORTED_IMAGE_TYPE":
                        ETotalText.SetError(GameController.LangManager.GetTextValue("Reg.WrongType"));
                        break;
                    case "IMAGE_IS_TOO_LARGE":
                        ETotalText.SetError(GameController.LangManager.GetTextValue("Reg.ToLarge"));
                        break;
                    default:
                        ETotalText.SetError(GameController.LangManager.GetTextValue("Reg.TotalError"));
                        break;
                }
            }
            else
            {
                

                if (obj.Contains("error"))
                {
                    if (_canvasGroup != null) _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
                    switch (obj["error"].StringValue)
                    {
                        case "email_is_used":
                            EEmailText.SetError(GameController.LangManager.GetTextValue("Reg.MailExist"));
                            break;
                    }
                }else
                {
                    if (!obj.Contains("status"))
                    {
                        ShowError("login", GameController.LangManager.GetTextValue("Reg.LoginExistError"));
                    }
                }

            }
        }
    }
   

    public IEnumerator LoadAvatarCoroutine(string str)
    {
        WWW wwwAvatar = new WWW(str);
        yield return wwwAvatar;
        AvatarImage.texture = wwwAvatar.texture;
        AvatarImg = wwwAvatar.texture;
    }
    public void GetCapcha(string link)
    {
        //   this.cube1.material.mainTexture = null;
        UCSS.HTTP.GetTexture(link, new EventHandlerHTTPTexture(this.OnCapchaDownloaded), new EventHandlerServiceError(this.OnHTTPError), new EventHandlerServiceTimeOut(this.OnHTTPTimeOut), 10);
    }

    void OnCapchaDownloaded(Texture texture, string transactionId)
    {
        //        Debug.Log("[RegPanel] [OnCapchaDownloaded] texture.width = " + texture.width + ", texture.height = " + texture.height);

        CapchaImage.texture = texture;
        // this.cube1.material.mainTexture = texture;
    }
    void OnHTTPError(string error, string transactionId)
    {
        Debug.LogError("[RegPanel] [OnHTTPError] error = " + error + " (transaction [" + transactionId + "])");
    }


    void OnHTTPTimeOut(string transactionId)
    {
        Debug.LogError("[RegPanel] [OnHTTPTimeOut] transactionId = " + transactionId);
        UCSS.HTTP.RemoveTransaction(transactionId);
    }
    public void ShowError(string field, string msg)
    {
        switch (field)
        {
            case "login":
                if (ELogin != null) ELogin.SetError(msg);
                break;
            case "pass":
                if (EPass != null) EPass.SetError(msg);
                break;
            case "pass2":
                if (EPass2 != null) EPass2.SetError(msg);
                break;
            case "email":
                if (EEmailText != null) EEmailText.SetError(msg);
                break;
            case "phone":
                if (EPhoneText != null) EPhoneText.SetError(msg);
                break;
            case "bdate":
                if (EBdateText != null) EBdateText.SetError(msg);
                break;
            case "country":
                if (ECountryText != null) ECountryText.SetError(msg);
                break;
            case "fio":
                if (EFioText != null) EFioText.SetError(msg);
                break;
            case "kod":
                if (EKodText != null) EKodText.SetError(msg);
                break;
            case "avatar":
                if (EAvatarText != null) EAvatarText.SetError(msg);
                break;
            default:
                if (ETotalText != null) ETotalText.SetError(msg);
                break;
        }
    }
    /// <summary>
    /// проверяем логин на доступность
    /// </summary>
    public void SendCheckLogin(string login)
    {
        if (GameController.Token != "")
        {
            string link = Net.ServerURL + "/api/users/isloginused?username=" + login + "&access-token=" + GameController.Token;
            print("SendCheckLogin " + link);
            UCSS.HTTP.GetString(link, new EventHandlerHTTPString(OnCheckLogin), new EventHandlerServiceError(OnHTTPError),
                new EventHandlerServiceTimeOut(OnHTTPTimeOut), 10);
        }
    }

    public void OnCheckLogin(string text, string transactionid)
    {
        IJSonMutableObject obj = Net.ReadPack(text);
        if (obj != null)
        {
            if (obj["isLoginUsed"].BooleanValue)
            {
                print("логин занят " + text);
                ShowError("login", GameController.LangManager.GetTextValue("Reg.LoginExistError"));
            }
            else
            {
                print("логин свободен");
                ShowError("login", "");
            }
        }
    }

}
