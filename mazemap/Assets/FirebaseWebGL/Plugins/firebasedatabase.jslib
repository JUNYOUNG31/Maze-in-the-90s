mergeInto(LibraryManager.library, {
 
    PostJSON: function(path, value, objectName, callback, fallback) {
        var parsedPath = Pointer_stringify(path);
        var parsedValue = Pointer_stringify(value);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
 
            firebase.database().ref(parsedPath).set(parsedValue).then(function(unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedValue + " was posted to " + parsedPath);
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    GetJSON: function(path, objectName, callback, fallback) {
        var parsedPath = Pointer_stringify(path);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
 
            firebase.database().ref(parsedPath).once('value').then(function(snapshot) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(snapshot.val()));
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    PushJSON: function(path, value, objectName, callback, fallback) {
        var parsedPath = Pointer_stringify(path);
        var parsedValue = Pointer_stringify(value);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
 
            firebase.database().ref(parsedPath).push().set(parsedValue).then(function(unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedValue + " was pushed to " + parsedPath);
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    UpdateJSON: function(path, value, objectName, callback, fallback) {
        var parsedPath = Pointer_stringify(path);
        var parsedValue = Pointer_stringify(value);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        var postData = { Score : parsedValue };
 
        try {
 
            firebase.database().ref(parsedPath).update(postData).then(function(unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedValue + " was updated in " + parsedPath);
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    DeleteJSON: function(path, objectName, callback, fallback) {
        var parsedPath = Pointer_stringify(path);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
 
            firebase.database().ref(parsedPath).remove().then(function(unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedPath + " was deleted");
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    //���� ������ �� ����Ƽ���� ���� ������ ���̾�̽��� ������(���常)
    PostGameRecord: function(json) {
    //unity���� string���� ���� json
    var parsedJSON = Pointer_stringify(json);
        
    //string
    console.log(parsedJSON);
    console.log(typeof parsedJSON);

    //string�� json ������Ʈ�� ��ȯ
    var obj = JSON.parse(parsedJSON);
    console.log(obj);
    console.log(typeof obj);
    
    //������Ʈ���� �ʿ��� value ���� ã�Ƽ� ������ ����
    //const mode = obj.gameMode;
    //const map = obj.gameMap;
    const name = obj.nickName;
    const time = obj.time;

    //DB ���� ��δ� rank/���(0/1)/��/�г���
    //������ �����ʹ� {time : �ɸ� �ð�}
    var rankRef = firebase.database().ref('rank/' + '0' + '/' + 'forest1' + '/' + name); //��ü ��ŷ
    var recordRef = firebase.database().ref('record/' + name + '/' + '0' + '/' + 'forest1'); //���� ��������

    //{ name: ��¼��, time: 12 }
    var postData = new Object();
    postData.name = name;
    postData.time = time;

    // ��ü ��ŷ ���̺�� ���� ���� ���̺��� ������Ʈ
    firebase.database().ref(rankRef).update(postData).then(function(unused) {
        console.log('rank post completed!');
    });

    firebase.database().ref(recordRef).update(postData).then(function(unused) {
        console.log('record post completed!');
    });

   },

   //��ŷ������ �������� ��
   SetGameRecord: function() {
    //TOP 10 ��ŷ �� �о����
    firebase.database().ref('rank/' + '0' + '/' + 'forest1').orderByChild('time').limitToFirst(10).once('value').then(function(list) {
    //���ĵ� �����͸� �������� ���ؼ� �ϳ��ϳ��� ����
     list.forEach(function (score) {
        console.log(score.val());
        //���ӵ����͸� �ٽ� ����Ƽ�� ����
        window.unityInstance.SendMessage('RankingHandler', 'SetUp', JSON.stringify(score.val()));
        });
     });
   },

});