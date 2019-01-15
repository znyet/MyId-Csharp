# MyId-Csharp

## MyIdClient
```c#
using MyIdClient;

//MyId is thread safe,please use single instance
public class MyIdHelper
{
    private static readonly object _lock = new object();
    private static MyId _myId;

    public static MyId myId
    {
        get
        {
            if (_myId == null)
            {
                lock (_lock)
                {
                    //if (_myId == null)
                    //    _myId = new MyId("127.0.0.1", 8123);

                    if (_myId == null)
                        _myId = new MyId("server=127.0.0.1;port=8123;pwd=123456;maxPool=100;timeout=1000;lifetime=0");
                }
            }
            return _myId;
        }

    }

```

Now you can get id from the server.

```c#
string id = MyIdHelper.myId.GetGuid();
//da96b455-91e1-4bab-8638-18f8eb1955ee

string id = MyIdHelper.myId.GetGuid(2); //return 2 id split with ,
//f706296d-be31-4908-a6bd-4ce3abebf548,40353568-d4a3-4e9c-b3bb-6045612f7f55

string id = MyIdHelper.myId.GetGuidToN();
//7e113ffa71e649448814140f32d8cdb7

string id = MyIdHelper.myId.GetGuidToN(2); //return 2 id split with ,
//5593e0a617d94b8792976e5eb39a4b53,0b1a41765a6b4f6fbfe51db8e70f57e2

string id = MyIdHelper.myId.GetObjectId();
//5c3d292b922fdb092429ec2c

string id = MyIdHelper.myId.GetObjectId(2); //return 2 id split with ,
//5c3d292b922fdb092429ec2d,5c3d292b922fdb092429ec2e

string id = MyIdHelper.myId.GetSnowflakeId(); //Twitter SnowFlake
//1084970533713346560

string id = MyIdHelper.myId.GetBase16Id();  //Base36 -> 16
//F8JP8XBW03DGNCJ

string id = MyIdHelper.myId.GetBase20Id();  //Base36 -> 20
//0F8JP8XBWJMDGASC3821

string id = MyIdHelper.myId.GetBase25Id();  //Base36 -> 25
//F8JP8XBW03DGNCJ
```
