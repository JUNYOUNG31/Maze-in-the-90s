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

    //��ŷ������ Post test
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
    const mode = obj.gameMode;
    const map = obj.gameMap;
    const name = obj.nickName;
    const time = obj.time;

    //DB ���� ��δ� records/���/��
    //������ �����ʹ� {�г��� : �ɸ� �ð�}
    var recordRef = firebase.database().ref('records/' + mode + '/' + map);

    var postData = {};
    postData[name] = time;

    firebase.database().ref(recordRef).set(postData).then(function(unused) {
        console.log('gamedata post completed!')
    });
    
    },

});