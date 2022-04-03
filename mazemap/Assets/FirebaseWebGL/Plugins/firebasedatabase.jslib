mergeInto(LibraryManager.library, {
 
    //�г��� �ߺ��˻�
    CheckNickname: function(name) {
    
    //���� �õ��ϴ� �г���
    var parsedNick = Pointer_stringify(name);
    console.log(parsedNick);
    // ������ ���
    var nameRef = firebase.database().ref('users'); //��ü ����
    // ��� (0 -> �ߺ� ���� ���� �Ұ���, 1 -> �ߺ� ���� ���� ����)
    var result = 1

    firebase.database().ref(nameRef).once('value').then(function(list) {
    //�ϳ��� ����
     list.forEach(function (user) {
        console.log(user.val());
        console.log(user.val().nickname);

        //�ߺ� ����
        if (user.val().nickname == parsedNick){
            console.log('�ߺ�');
            result = 0
        }
      });
      //�˻� �� ����Ƽ�� ��� ����
      console.log(result);
      console.log(typeof result);
      window.unityInstance.SendMessage('SignUpHandler', 'CheckedName', result);
     });

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

    //��ü ��ŷ ���̺� ������Ʈ(�ش� ������ ����� �̹� �ִ� ��� �� ª�� ������� ��ü�� ��)
    rankRef.get().then(function(snapshot) {
    //�ش� ��ο� ����� �̹� ����
    if (snapshot.exists()) {
        console.log(snapshot.val());

        //�ð� ��
        //������ �ִ� ����� ���ų� �� ª�ٸ� �������� ����
        if (snapshot.val().time <= time){
            console.log('time not replaced...');
        } 
        //��� ���������� ������ ������ ��ü��
        else{
            firebase.database().ref(rankRef).update(postData).then(function(unused) {
            console.log('time replaced!');
            });
        }


        //�ش� ��ο� ����� ����(�ش� ���, �ʿ��� ù ������ ���)
    } else {
        firebase.database().ref(rankRef).update(postData).then(function(unused) {
        console.log('rank post completed!');
        });
    }
    });
    
    //���� ���� ���̺� ������Ʈ(����� ����� ����)
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