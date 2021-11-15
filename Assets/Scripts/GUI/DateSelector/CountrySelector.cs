using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class CountrySelector : DateSelector
{
    /// <summary>
    /// ссылки на конпки
    /// </summary>
    private List<DropDownItem> _buttonsList;

    public int SelectedID = 1;



    public VerticalLayoutGroup Content;
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
    public List<string> DList { get; set; }

    public void Init()
    {

        DList = new List<string>();
        DList.Add("");
        for (int i = 0; i < CountryList.Length; i++)
        {
            DList.Add(CountryList[i]);
        }
        ActiveItem = CountryList[0];
        DList.Add("");
        InitCountryList();
    }




    /// <summary>
    /// инициализируем кнопки в меню
    /// </summary>
    /// <param name="items"></param>
    public new void InitCountryList()
    {
        List<string> items = DList;
        //Items = items;
        if (_buttonsList != null)
        {
            foreach (DropDownItem button in _buttonsList)
            {
                Destroy(button.gameObject);
            }
        }
        _buttonsList = new List<DropDownItem>();
        GameObject go;
        DropDownItem ddi;
        int i = 0;
        foreach (string s in items)
        {
            go = (GameObject)Instantiate(ItemLink);
            go.transform.SetParent(Content.gameObject.transform);
            go.gameObject.SetActive(true);
            ddi = go.GetComponent<DropDownItem>();
            ddi.Id = i;
            ddi.LabelText.text = s;
            _buttonsList.Add(ddi);
            i++;
        }

    }




    public override void SelectItem(ETip tip, int target)
    {
        if (tip == ETip.Country)
        {
            //print("!!!" + target+":"+CountryList[target]);
            SelectedID = target - 1;
            ActiveItem = CountryList[target - 1];
        }
    }

    public override void OnOk()
    {
        TargetField.text = ActiveItem;
        gameObject.SetActive(false);
    }

    public string ActiveItem { get; set; }

    public override void OnCancel()
    {
        gameObject.SetActive(false);
    }

}
