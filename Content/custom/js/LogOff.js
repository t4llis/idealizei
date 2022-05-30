function signOutGoogle() {
    var auth2 = gapi.auth2.getAuthInstance();
    auth2.signOut().then(function () {
        console.log('Usuario desconectado google.');
        auth2.disconnect();
    });
}    