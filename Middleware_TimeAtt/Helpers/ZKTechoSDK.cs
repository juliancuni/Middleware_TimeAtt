using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_TimeAtt
{
    class ZKTechoSDK
    {
        public zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();
        private static bool bIsConnected = false;//the boolean value identifies whether the device is connected
        private static int iMachineNumber = 1;
        private static int idwErrorCode = 0;
        bool bAddControl = true;        //Get all user's ID

        #region ConnectDevice

        public bool GetConnectState()
        {
            return bIsConnected;
        }

        public void SetConnectState(bool state)
        {
            bIsConnected = state;
            //connected = state;
        }

        public int GetMachineNumber()
        {
            return iMachineNumber;
        }

        public void SetMachineNumber(int Number)
        {
            iMachineNumber = Number;
        }

        public int sta_ConnectTCP(string ip, string port, string commKey)
        {
            if (ip == "" || port == "" || commKey == "")
            {
                Logger.WriteLog("ERROR: ZKT Ip, Port ose Commkey s'mund te lihen bosh.");
                return -1;// ip or port is null
            }

            if (Convert.ToInt32(port) <= 0 || Convert.ToInt32(port) > 65535)
            {
                Logger.WriteLog("ERROR: Port duhet te jete numer!");
                return -1;
            }

            if (Convert.ToInt32(commKey) < 0 || Convert.ToInt32(commKey) > 999999)
            {
                Logger.WriteLog("ERROR: CommKey duhet te jete numer!");
                return -1;
            }

            int idwErrorCode = 0;

            axCZKEM1.SetCommPassword(Convert.ToInt32(commKey));

            if (bIsConnected == true)
            {
                axCZKEM1.Disconnect();
                sta_UnRegRealTime();
                SetConnectState(false);
                Logger.WriteLog("INFO: U shkeputa me ZKTeco");
                //connected = false;
                return -2; //disconnect
            }

            if (axCZKEM1.Connect_Net(ip, Convert.ToInt32(port)) == true)
            {
                SetConnectState(true);
                sta_RegRealTime();
                Logger.WriteLog("INFO: Lidhja me ZKTeco u krye.");
                return 1;
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
                Logger.WriteLog("ERROR: Nuk mund te lidhem me pajisjen. ErrCode=" + idwErrorCode.ToString());
                return idwErrorCode;
            }
        }

        public void sta_DisConnect()
        {
            if (GetConnectState() == true)
            {
                axCZKEM1.Disconnect();
                sta_UnRegRealTime();
                Logger.WriteLog("INFO: ZKTeco u shkeput me sukses");
            }
        }

        #endregion

        #region RealTimeEvent

        public void sta_UnRegRealTime()
        {
            this.axCZKEM1.OnFinger -= new zkemkeeper._IZKEMEvents_OnFingerEventHandler(axCZKEM1_OnFinger);
            this.axCZKEM1.OnVerify -= new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(axCZKEM1_OnVerify);
            this.axCZKEM1.OnAttTransactionEx -= new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(axCZKEM1_OnAttTransactionEx);
            this.axCZKEM1.OnFingerFeature -= new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(axCZKEM1_OnFingerFeature);
            this.axCZKEM1.OnDeleteTemplate -= new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(axCZKEM1_OnDeleteTemplate);
            this.axCZKEM1.OnNewUser -= new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(axCZKEM1_OnNewUser);
            this.axCZKEM1.OnHIDNum -= new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(axCZKEM1_OnHIDNum);
            this.axCZKEM1.OnAlarm -= new zkemkeeper._IZKEMEvents_OnAlarmEventHandler(axCZKEM1_OnAlarm);
            this.axCZKEM1.OnDoor -= new zkemkeeper._IZKEMEvents_OnDoorEventHandler(axCZKEM1_OnDoor);
            this.axCZKEM1.OnEnrollFingerEx -= new zkemkeeper._IZKEMEvents_OnEnrollFingerExEventHandler(axCZKEM1_OnEnrollFingerEx);
            this.axCZKEM1.OnWriteCard += new zkemkeeper._IZKEMEvents_OnWriteCardEventHandler(axCZKEM1_OnWriteCard);
            this.axCZKEM1.OnEmptyCard += new zkemkeeper._IZKEMEvents_OnEmptyCardEventHandler(axCZKEM1_OnEmptyCard);
            this.axCZKEM1.OnHIDNum += new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(axCZKEM1_OnHIDNum);
            this.axCZKEM1.OnAttTransaction -= new zkemkeeper._IZKEMEvents_OnAttTransactionEventHandler(axCZKEM1_OnAttTransaction);
            this.axCZKEM1.OnKeyPress += new zkemkeeper._IZKEMEvents_OnKeyPressEventHandler(axCZKEM1_OnKeyPress);
            this.axCZKEM1.OnEnrollFinger += new zkemkeeper._IZKEMEvents_OnEnrollFingerEventHandler(axCZKEM1_OnEnrollFinger);

        }

        public int sta_RegRealTime()
        {
            if (GetConnectState() == false)
            {
                Logger.WriteLog("WARNING: Duhet te lidheni me ZKTeco me pare");
                return -1024;
            }

            int ret = 0;

            if (axCZKEM1.RegEvent(GetMachineNumber(), 65535))//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
            {
                //common interface
                this.axCZKEM1.OnFinger += new zkemkeeper._IZKEMEvents_OnFingerEventHandler(axCZKEM1_OnFinger);
                this.axCZKEM1.OnVerify += new zkemkeeper._IZKEMEvents_OnVerifyEventHandler(axCZKEM1_OnVerify);
                this.axCZKEM1.OnFingerFeature += new zkemkeeper._IZKEMEvents_OnFingerFeatureEventHandler(axCZKEM1_OnFingerFeature);
                this.axCZKEM1.OnDeleteTemplate += new zkemkeeper._IZKEMEvents_OnDeleteTemplateEventHandler(axCZKEM1_OnDeleteTemplate);
                this.axCZKEM1.OnNewUser += new zkemkeeper._IZKEMEvents_OnNewUserEventHandler(axCZKEM1_OnNewUser);
                this.axCZKEM1.OnHIDNum += new zkemkeeper._IZKEMEvents_OnHIDNumEventHandler(axCZKEM1_OnHIDNum);
                this.axCZKEM1.OnAlarm += new zkemkeeper._IZKEMEvents_OnAlarmEventHandler(axCZKEM1_OnAlarm);
                this.axCZKEM1.OnDoor += new zkemkeeper._IZKEMEvents_OnDoorEventHandler(axCZKEM1_OnDoor);

                //only for color device
                this.axCZKEM1.OnAttTransactionEx += new zkemkeeper._IZKEMEvents_OnAttTransactionExEventHandler(axCZKEM1_OnAttTransactionEx);
                this.axCZKEM1.OnEnrollFingerEx += new zkemkeeper._IZKEMEvents_OnEnrollFingerExEventHandler(axCZKEM1_OnEnrollFingerEx);

                //only for black&white device
                this.axCZKEM1.OnAttTransaction -= new zkemkeeper._IZKEMEvents_OnAttTransactionEventHandler(axCZKEM1_OnAttTransaction);
                this.axCZKEM1.OnWriteCard += new zkemkeeper._IZKEMEvents_OnWriteCardEventHandler(axCZKEM1_OnWriteCard);
                this.axCZKEM1.OnEmptyCard += new zkemkeeper._IZKEMEvents_OnEmptyCardEventHandler(axCZKEM1_OnEmptyCard);
                this.axCZKEM1.OnKeyPress += new zkemkeeper._IZKEMEvents_OnKeyPressEventHandler(axCZKEM1_OnKeyPress);
                this.axCZKEM1.OnEnrollFinger += new zkemkeeper._IZKEMEvents_OnEnrollFingerEventHandler(axCZKEM1_OnEnrollFinger);


                ret = 1;
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
                ret = idwErrorCode;

                if (idwErrorCode != 0)
                {
                    Logger.WriteLog("WARNING: RegEvent failed,ErrorCode: " + idwErrorCode.ToString());
                }
                else
                {
                    Logger.WriteLog("ERROR: No data returns from terminal!");
                }
            }
            return ret;
        }

        //When you are enrolling your finger,this event will be triggered.
        void axCZKEM1_OnEnrollFingerEx(string EnrollNumber, int FingerIndex, int ActionResult, int TemplateLength)
        {
            Pergjigje pergjigje = new Pergjigje();
            pergjigje.Komanda = "New_Finger_Skanned";
            string msg;
            if (ActionResult == 0)
            {
                msg = "SUCCESS: Enroll finger succeed. UserID=" + EnrollNumber.ToString() + "...FingerIndex=" + FingerIndex.ToString();
                Logger.WriteLog(msg);
                pergjigje.Sukses = true;
                pergjigje.Mesazh = msg;
                pergjigje.AttId = int.Parse(EnrollNumber);
            }
            else
            {
                msg = "ERROR: Enroll finger failed. Result=" + ActionResult.ToString();
                Logger.WriteLog(msg);
                pergjigje.Sukses = false;
                pergjigje.Mesazh = msg;
            }
            string URI = ConfReader.Read("api") + ConfReader.Read("middlewareendpoint") + "?access_token=" + AccessToken.Get().id;
            string pergjigjeStringify = JsonConvert.SerializeObject(pergjigje);
            HttpCallApi.Post(URI, pergjigjeStringify);
            //throw new NotImplementedException();
        }

        //Door sensor event
        void axCZKEM1_OnDoor(int EventType)
        {
            Logger.WriteLog("Door opened" + "...EventType=" + EventType.ToString());

            throw new NotImplementedException();
        }

        //When the dismantling machine or duress alarm occurs, trigger this event.
        void axCZKEM1_OnAlarm(int AlarmType, int EnrollNumber, int Verified)
        {
            Logger.WriteLog("WARNING: Alarm triggered" + "...AlarmType=" + AlarmType.ToString() + "...EnrollNumber=" + EnrollNumber.ToString() + "...Verified=" + Verified.ToString());

            throw new NotImplementedException();
        }

        //When you swipe a card to the device, this event will be triggered to show you the card number.
        void axCZKEM1_OnHIDNum(int CardNumber)
        {
            Logger.WriteLog("Card event" + "...Cardnumber=" + CardNumber.ToString());

            throw new NotImplementedException();
        }

        //When you have enrolled a new user,this event will be triggered.
        void axCZKEM1_OnNewUser(int EnrollNumber)
        {
            Logger.WriteLog("INFO: Enroll a　new user" + "...UserID=" + EnrollNumber.ToString());

            throw new NotImplementedException();
        }

        //When you have deleted one one fingerprint template,this event will be triggered.
        void axCZKEM1_OnDeleteTemplate(int EnrollNumber, int FingerIndex)
        {
            Logger.WriteLog("INFO: Delete a finger template" + "...UserID=" + EnrollNumber.ToString() + "..FingerIndex=" + FingerIndex.ToString());

            throw new NotImplementedException();
        }

        //When you have enrolled your finger,this event will be triggered and return the quality of the fingerprint you have enrolled
        void axCZKEM1_OnFingerFeature(int Score)
        {
            Logger.WriteLog("INFO: Press finger score=" + Score.ToString());

            //throw new NotImplementedException();
        }

        //If your fingerprint(or your card) passes the verification,this event will be triggered,only for color device
        void axCZKEM1_OnAttTransactionEx(string EnrollNumber, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second, int WorkCode)
        {
            string time = Year + "-" + Month + "-" + Day + " " + Hour + ":" + Minute + ":" + Second;
            ApiTimeAttendances.RegAttendance(EnrollNumber, AttState);
            //Logger.WriteLog("INFO: Verify OK.UserID=" + EnrollNumber + " isInvalid=" + IsInValid.ToString() + " state=" + AttState.ToString() + " verifystyle=" + VerifyMethod.ToString() + " time=" + time);

            //throw new NotImplementedException();
        }

        //If your fingerprint(or your card) passes the verification,this event will be triggered,only for black%white device
        private void axCZKEM1_OnAttTransaction(int EnrollNumber, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second)
        {
            string time = Year + "-" + Month + "-" + Day + " " + Hour + ":" + Minute + ":" + Second;
            Logger.WriteLog("Verify OK.UserID=" + EnrollNumber.ToString() + " isInvalid=" + IsInValid.ToString() + " state=" + AttState.ToString() + " verifystyle=" + VerifyMethod.ToString() + " time=" + time);

            throw new NotImplementedException();
        }

        //After you have placed your finger on the sensor(or swipe your card to the device),this event will be triggered.
        //If you passes the verification,the returned value userid will be the user enrollnumber,or else the value will be -1;
        void axCZKEM1_OnVerify(int UserID)
        {
            if (UserID != -1)
            {
                //Logger.WriteLog("INFO: User fingerprint verified... UserID=" + UserID.ToString());
            }
            else
            {
                Logger.WriteLog("ERROR: Deshtim ne verifikim te gishtit ");
            }

            //throw new NotImplementedException();
        }

        //When you place your finger on sensor of the device,this event will be triggered
        void axCZKEM1_OnFinger()
        {
            Logger.WriteLog("INFO: Skan Gisht...");

            //throw new NotImplementedException();
        }

        //When you have written into the Mifare card ,this event will be triggered.
        void axCZKEM1_OnWriteCard(int iEnrollNumber, int iActionResult, int iLength)
        {
            if (iActionResult == 0)
            {
                Logger.WriteLog("Write Mifare Card OK" + "...EnrollNumber=" + iEnrollNumber.ToString() + "...TmpLength=" + iLength.ToString());
            }
            else
            {
                Logger.WriteLog("...Write Failed");
            }
        }

        //When you have emptyed the Mifare card,this event will be triggered.
        void axCZKEM1_OnEmptyCard(int iActionResult)
        {
            if (iActionResult == 0)
            {
                Logger.WriteLog("Empty Mifare Card OK...");
            }
            else
            {
                Logger.WriteLog("Empty Failed...");
            }
        }

        //When you press the keypad,this event will be triggered.
        void axCZKEM1_OnKeyPress(int iKey)
        {
            Logger.WriteLog("INFO: RTEvent OnKeyPress Has been Triggered, Key: " + iKey.ToString());
        }

        //When you are enrolling your finger,this event will be triggered.
        void axCZKEM1_OnEnrollFinger(int EnrollNumber, int FingerIndex, int ActionResult, int TemplateLength)
        {
            //Pergjigje pergjigje = new Pergjigje();
            //pergjigje.Komanda = "New_Finger_Skanned";
            string msg;

            if (ActionResult == 0)
            {
                msg = "INFO: Enroll finger succeed. UserID=" + EnrollNumber + "...FingerIndex=" + FingerIndex.ToString();
                //pergjigje.Sukses = false;
                //pergjigje.Mesazh = msg;
                Logger.WriteLog(msg);
            }
            else
            {
                msg = "INFO: +Enroll finger failed. Result=" + ActionResult.ToString();
                //pergjigje.Sukses = false;
                //pergjigje.Mesazh = msg;
                Logger.WriteLog(msg);
            }
            //string URI = ConfReader.Read("api") + ConfReader.Read("middlewareendpoint") + "?access_token=" + AccessToken.Get().id;
            //string pergjigjeStringify = JsonConvert.SerializeObject(pergjigje);
            //HttpCallApi.Post(URI, pergjigjeStringify);
            //throw new NotImplementedException();
        }

        #endregion

        #region UserInfo
        public int sta_OnlineEnroll(string txtUserID, string cbFingerIndex, string cbFlag)
        {
            /*
            txtUserID = "1";
            cbFingerIndex = "9";
            cbFlag = "0";
            */
            if (GetConnectState() == false)
            {
                Logger.WriteLog("ERROR: Nuk jeni lidhur me pajisjen");
                return -1024;
            }

            if (txtUserID == "" || cbFingerIndex == "" || cbFlag == "")
            {
                Logger.WriteLog("ERROR: Plotesoni te gjitha fushat e kerkuara. userId, fingerIndex dhe cbFlag");
                return -1023;
            }

            int iPIN2Width = 0;
            int iIsABCPinEnable = 0;
            int iT9FunOn = 0;
            string strTemp = "";
            axCZKEM1.GetSysOption(GetMachineNumber(), "~PIN2Width", out strTemp);
            iPIN2Width = Convert.ToInt32(strTemp);
            axCZKEM1.GetSysOption(GetMachineNumber(), "~IsABCPinEnable", out strTemp);
            iIsABCPinEnable = Convert.ToInt32(strTemp);
            axCZKEM1.GetSysOption(GetMachineNumber(), "~T9FunOn", out strTemp);
            iT9FunOn = Convert.ToInt32(strTemp);

            /*
            axCZKEM1.GetDeviceInfo(iMachineNumber, 76, ref iPIN2Width);
            axCZKEM1.GetDeviceInfo(iMachineNumber, 77, ref iIsABCPinEnable);
            axCZKEM1.GetDeviceInfo(iMachineNumber, 78, ref iT9FunOn);
             */

            if (txtUserID.Length > iPIN2Width)
            {
                Logger.WriteLog("ERROR: User ID error! The max length is " + iPIN2Width.ToString());
                Logger.WriteLog("INFO: Restarting Middlware Service...");
                Environment.Exit(1);
                return -1022;
            }

            if (iIsABCPinEnable == 0 || iT9FunOn == 0)
            {
                if (txtUserID.Substring(0, 1) == "0")
                {
                    Logger.WriteLog("ERROR: User ID error! The first letter can not be as 0");
                    return -1022;
                }

                foreach (char tempchar in txtUserID.ToCharArray())
                {
                    if (!(char.IsDigit(tempchar)))
                    {
                        Logger.WriteLog("ERROR: User ID error! User ID only support digital");
                        return -1022;
                    }
                }
            }

            int idwErrorCode = 0;
            string sUserID = txtUserID;
            int iFingerIndex = Convert.ToInt32(cbFingerIndex);
            int iFlag = Convert.ToInt32(cbFlag);

            axCZKEM1.CancelOperation();
            //If the specified index of user's templates has existed ,delete it first
            axCZKEM1.SSR_DelUserTmpExt(iMachineNumber, sUserID, iFingerIndex);
            if (axCZKEM1.StartEnrollEx(sUserID, iFingerIndex, iFlag))
            {
                Logger.WriteLog("INFO: Skano gisht te ri per: " + sUserID + " GishtID=" + iFingerIndex.ToString() + " Flag=" + iFlag.ToString());
                if (axCZKEM1.StartIdentify())
                {
                    Logger.WriteLog("INFO: UserID" + sUserID);
                }
                ;//After enrolling templates,you should let the device into the 1:N verification condition
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
                Logger.WriteLog("ERRORs: Operation failed,ErrorCode=" + idwErrorCode.ToString());
            }

            return 1;
        }
        public int sta_SetUserInfo(string txtUserID, string txtName, string cbPrivilege, string txtPassword)
        {
            Pergjigje pergjigje = new Pergjigje();
            pergjigje.Komanda = "New_Finger_Skanned";
            string msg;

            if (GetConnectState() == false)
            {
                msg = "ERROR: Nuk jeni lidhur me pajisjen";
                Logger.WriteLog(msg);
                pergjigje.Sukses = false;
                pergjigje.Mesazh = msg;
                return -1024;
            }

            if (txtUserID == "" || cbPrivilege == "")
            {
                msg = "ERROR: Plotesoni te gjitha fushat e kerkuara. userId, cbPrivilege";
                Logger.WriteLog(msg);
                pergjigje.Sukses = false;
                pergjigje.Mesazh = msg;
                return -1023;
            }
            int iPrivilege = int.Parse(cbPrivilege);

            //bool bFlag = false;
            if (iPrivilege == 5)
            {
                msg = "*User Defined Role is Error! Please Register again!";
                Logger.WriteLog(msg);
                pergjigje.Sukses = false;
                pergjigje.Mesazh = msg;
                return -1023;
            }

            /*
            if(iPrivilege == 4)
            {
                axCZKEM1.IsUserDefRoleEnable(iMachineNumber, 4, out bFlag);

                if (bFlag == false)
                {
                    Logger.WriteLog("*User Defined Role is unable!");
                    return -1023;
                }
            }
             */
            //Logger.WriteLog("[func IsUserDefRoleEnable]Temporarily unsupported");

            int iPIN2Width = 0;
            int iIsABCPinEnable = 0;
            int iT9FunOn = 0;
            string strTemp = "";
            axCZKEM1.GetSysOption(GetMachineNumber(), "~PIN2Width", out strTemp);
            iPIN2Width = Convert.ToInt32(strTemp);
            axCZKEM1.GetSysOption(GetMachineNumber(), "~IsABCPinEnable", out strTemp);
            iIsABCPinEnable = Convert.ToInt32(strTemp);
            axCZKEM1.GetSysOption(GetMachineNumber(), "~T9FunOn", out strTemp);
            iT9FunOn = Convert.ToInt32(strTemp);
            /*
            axCZKEM1.GetDeviceInfo(iMachineNumber, 76, ref iPIN2Width);
            axCZKEM1.GetDeviceInfo(iMachineNumber, 77, ref iIsABCPinEnable);
            axCZKEM1.GetDeviceInfo(iMachineNumber, 78, ref iT9FunOn);
            */
            if (txtUserID.Length > iPIN2Width)
            {
                Logger.WriteLog("*User ID error! The max length is " + iPIN2Width.ToString());
                return -1022;
            }

            if (iIsABCPinEnable == 0 || iT9FunOn == 0)
            {
                if (txtUserID.Substring(0, 1) == "0")
                {
                    msg = "*User ID error! The first letter can not be as 0";
                    Logger.WriteLog(msg);
                    pergjigje.Sukses = false;
                    pergjigje.Mesazh = msg;
                    return -1022;
                }

                foreach (char tempchar in txtUserID.ToCharArray())
                {
                    if (!(char.IsDigit(tempchar)))
                    {
                        msg = "*User ID error! User ID only support digital";
                        Logger.WriteLog(msg);
                        pergjigje.Sukses = false;
                        pergjigje.Mesazh = msg;
                        return -1022;
                    }
                }
            }

            int idwErrorCode = 0;
            string sdwEnrollNumber = txtUserID;
            string sName = txtName.Trim();
            string sPassword = txtPassword;

            bool bEnabled = true;
            /*if (iPrivilege == 4)
            {
                bEnabled = false;
                iPrivilege = 0;
            }
            else
            {
                bEnabled = true;
            }*/

            axCZKEM1.EnableDevice(iMachineNumber, false);
            //axCZKEM1.SetStrCardNumber(sCardnumber);//Before you using function SetUserInfo,set the card number to make sure you can upload it to the device
            if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, sdwEnrollNumber, sName, sPassword, iPrivilege, bEnabled))//upload the user's information(card number included)
            {
                msg = "SUCCESS: Perdoruesi u regjistrua me sukses";
                Logger.WriteLog(msg);
                pergjigje.Sukses = true;
                pergjigje.Mesazh = msg;
                pergjigje.AttId = int.Parse(txtUserID);
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
                msg = "ERROR: *Operation failed,ErrorCode=" + idwErrorCode.ToString();
                Logger.WriteLog(msg);
                pergjigje.Sukses = false;
            }
            axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
            axCZKEM1.EnableDevice(iMachineNumber, true);
            string URI = ConfReader.Read("api") + ConfReader.Read("middlewareendpoint") + "?access_token=" + AccessToken.Get().id;
            string pergjigjeStringify = JsonConvert.SerializeObject(pergjigje);
            HttpCallApi.Post(URI, pergjigjeStringify);
            return 1;
        }
        #endregion
    }
}
