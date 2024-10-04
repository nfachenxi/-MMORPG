using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIlogin : MonoBehaviour {

	public InputField username;
	public InputField password;
	public Button buttonLogin;
	public Button buttonRegister;


	void Start () {
        UserService.Instance.OnLogin = this.OnLogin;
    }
    void OnLogin(SkillBridge.Message.Result result, string msg)
    {
        MessageBox.Show(string.Format("结果: {0} msg:{1}", result, msg));
    }

    void Update () {
		
	}

	public void OnClickLogin()
	{
		if (string.IsNullOrEmpty(this.username.text))
		{
			MessageBox.Show("请输入账号");
			return;
		}
		if (string.IsNullOrEmpty(this.password.text))
		{
			MessageBox.Show("请输入密码");
			return;
		}
		UserService.Instance.SendLogin(this.username.text, this.password.text);
	}
}
