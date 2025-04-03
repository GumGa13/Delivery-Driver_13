using JetBrains.Annotations;
using UnityEngine;

public class House
{
    public string tv = "거실 티비"; //모두가 볼 수 있다.
    private string diary = "비밀 다이효리";
    protected string secretKey = "집 비밀버노";

    public string GetDiary()
    {
        Driver driver = new Driver(); //driver.movespeed = 1;

        return diary;
    }
}
