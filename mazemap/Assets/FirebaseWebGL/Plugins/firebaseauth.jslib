mergeInto(LibraryManager.library, {

    //�κ� ���Խ� ���������� ��������
    CheckAuthState: function() {
        
        const user = firebase.auth().currentUser;

        if (user) {
            
            //���̾�̽����� ������ ���� ��������

            var userName = user.displayName;
            var photoURL = user.photoURL;

            console.log(typeof userName);
            console.log(typeof photoURL);

            //����Ƽ�� ���� (������) ������
            window.unityInstance.SendMessage('LobbyHandler', 'GetUsername', userName);
            window.unityInstance.SendMessage('LobbyHandler', 'GetPhotoURL', photoURL);
            
        
        } else {
            console.log('user signed out!');
            window.unityInstance.SendMessage('LobbyHandler', 'LoginScreen');
        }
    
    
    },

    //�ڵ��α��� Ȯ�� 
    CheckAutoLogin: function() {
        
        const user = firebase.auth().currentUser;

        if (user) {
            console.log('autologin!');
            window.unityInstance.SendMessage('LoginHandler', 'LobbyScreen');
        
        } else {
            console.log('user signed out!');
        }
    
    
    },
    

    //�̸��Ϸ� ����
	CreateUserWithEmailAndPassword: function(username, email, password, objectName, callback) {
        
        var parsedUsername = Pointer_stringify(username);
	    var parsedEmail = Pointer_stringify(email);
        var parsedPassword = Pointer_stringify(password);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
 
        try {
 
            firebase.auth().createUserWithEmailAndPassword(parsedEmail, parsedPassword).then(function (userCredential) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: signed up for " + parsedEmail);
                var user = userCredential.user;

                console.log(user);
                return user;

            }).then(function (user) {

                console.log('profile update start!!');
                console.log(user);
                
                //Firebase Auth�� ���
                user.updateProfile({
                displayName: parsedUsername,
                photoURL: "https://pbs.twimg.com/media/EFKdt0bWsAIfcj9.jpg"
                }).then(function (unused) {
                    console.log('profile update done!!');
                    firebase.auth().signOut();
                    window.unityInstance.SendMessage('SignUpHandler', 'LoginScreen');
                });

                //Realtime Database�� ���
                console.log('db ��� ����!!');
                firebase.database().ref('users/' + user.uid).set(
                {
                    nickname: parsedUsername,
                    email: parsedEmail,
                    profile_picture : "https://pbs.twimg.com/media/EFKdt0bWsAIfcj9.jpg"
                });

                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: signed up for " + parsedEmail);
                
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(error, Object.getOwnPropertyNames(error)) );
        }
	},
    

    //�̸��Ϸ� �α���
    SignInWithEmailAndPassword: function (email, password, objectName, callback, fallback) {
 
        var parsedEmail = Pointer_stringify(email);
        var parsedPassword = Pointer_stringify(password);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
 
            firebase.auth().signInWithEmailAndPassword(parsedEmail, parsedPassword).then(function (unused) {
                
                var user = firebase.auth().currentUser;
                console.log(user);

                window.unityInstance.SendMessage('LoginHandler', 'LobbyScreen');
                
                unityInstance.Module.SendMessage(parsedObjectName, parsedCallback, "Success: signed in for " + parsedEmail);

            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)) );
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)) );
        }
    },
    
    //���� ó�� �α���(���� ����Ʈ �̹����� ����)
    SignInWithGoogle: function (objectName, callback, fallback) {
 
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
            var provider = new firebase.auth.GoogleAuthProvider();
            firebase.auth().signInWithPopup(provider).then(function (unused) {
                
                var user = firebase.auth().currentUser;
                return user;

            }).then(function (user) {

                console.log('google profile update start!!');
                console.log(user);
                
                //Firebase Auth�� ���
                user.updateProfile({
                photoURL: "https://pbs.twimg.com/media/EFKdt0bWsAIfcj9.jpg"
                }).then(function (unused) {
                    console.log('profile update done!!');
                    window.unityInstance.SendMessage('SignUpHandler', 'LoginScreen');
                });

                //Realtime Database�� ���
                firebase.database().ref('users/' + user.uid).set({
                    nickname: user.displayName,
                    email: user.email,
                    profile_picture : "https://pbs.twimg.com/media/EFKdt0bWsAIfcj9.jpg"
                });

                unityInstance.Module.SendMessage(parsedObjectName, parsedCallback, "Success: signed in with Google!");
            }).catch(function (error) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });
 
        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },
    

    //���� ó�� �α���(���� ����Ʈ �̹����� ����)
    SignInWithGithub: function (objectName, callback, fallback) {
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
            var provider = new firebase.auth.GithubAuthProvider();
            firebase.auth().signInWithPopup(provider).then(function (unused) {

                var user = firebase.auth().currentUser;
                return user;

            }).then(function (user) {

                console.log('github profile update start!!');
                console.log(user);
                
                //Firebase Auth�� ���
                user.updateProfile({
                photoURL: "https://pbs.twimg.com/media/EFKdt0bWsAIfcj9.jpg"
                }).then(function (unused) {
                    console.log('profile update done!!');
                    window.unityInstance.SendMessage('SignUpHandler', 'LoginScreen');
                });

                //Realtime Database�� ���
                firebase.database().ref('users/' + user.uid).set({
                    nickname: user.displayName,
                    email: user.email,
                    profile_picture : "https://pbs.twimg.com/media/EFKdt0bWsAIfcj9.jpg"
                });

                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: signed in with Github!");
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    //���� �α���(���� ����x)
    LoginWithGoogle: function (objectName, callback, fallback) {
 
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
            var provider = new firebase.auth.GoogleAuthProvider();
            firebase.auth().signInWithPopup(provider).then(function (unused) {
                
                var user = firebase.auth().currentUser;
                console.log(user);

                window.unityInstance.SendMessage('LoginHandler', 'LobbyScreen');
                
                unityInstance.Module.SendMessage(parsedObjectName, parsedCallback, "Success: signed in with Google!");
            }).catch(function (error) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });
 
        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    //���� �α���(���� ����x)
    LoginWithGithub: function (objectName, callback, fallback) {
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
            var provider = new firebase.auth.GithubAuthProvider();
            firebase.auth().signInWithPopup(provider).then(function (unused) {

                var user = firebase.auth().currentUser;
                console.log(user);
                
                window.unityInstance.SendMessage('LoginHandler', 'LobbyScreen');

                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: signed in with Github!");
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },


    //�α׾ƿ�
    SignOut: function() {
        firebase.auth().signOut().then(function (unused) {
            window.unityInstance.SendMessage('LobbyHandler', 'LoginScreen')});
    },


    //���� ����
    UpdateProfilePicture: function(newProfile) {
        var newPfp = Pointer_stringify(newProfile);
        const user = firebase.auth().currentUser;

        var pfData = { profile_picture : newPfp };

        //Firebase Auth���� ������Ʈ
        user.updateProfile({
            photoURL: newPfp
            }).then(function (unused) {
                console.log('profile update done!!');
                window.unityInstance.SendMessage('LobbyHandler', 'ChangePfpSuccess');
            });

        //Realtime Database���� ������Ʈ
        firebase.database().ref('users/' + user.uid).update(pfData);
        
    },

    //��й�ȣ ����(����������)
    UpdatePw: function(newPw) {
        var nextPw = Pointer_stringify(newPw);
        const user = firebase.auth().currentUser;

        user.updatePassword(nextPw).then(function (unused) {
        // Update successful.
        console.log('pw update done!!');
        window.unityInstance.SendMessage('LobbyHandler', 'ChangePwSuccess');
        });
    },

    //��й�ȣ �缳��(�α��� ȭ�鿡�� ��� �ؾ��� ��)
    ResetPassword: function(email) {
        const user = firebase.auth().currentUser;
        var email = Pointer_stringify(email);

        firebase.auth().sendPasswordResetEmail(email).then(function (unused) {
        console.log('pw reset email sent!!');
        window.unityInstance.SendMessage('LoginHandler', 'EmailSentScreen', email);
        });
    },

    //ȸ��Ż��
    DeleteUser: function() {

    const user = firebase.auth().currentUser;

    //Realtime Database���� ����
    firebase.database().ref('users/' + user.uid).remove().then(function(unused) {
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedPath + " was deleted")});
    
    //Firebase Auth���� ����
    user.delete().then(function (unused) {
        window.unityInstance.SendMessage('LobbyHandler', 'DeleteUserSuccess')});
    
    },

 
});