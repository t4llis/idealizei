var startApp = function () {
    gapi.load('auth2', function () {            
        auth2 = gapi.auth2.init({
            client_id: '487456676189-9qah0tv2o7t3oojf6m3p1ktf5klm4h1r.apps.googleusercontent.com',
            cookiepolicy: 'single_host_origin',
        });
        loginGoogle(document.getElementById('btn_google'));
    });
};

function loginGoogle(element) {        
    auth2.attachClickHandler(element, {},
        function (googleUser) {
            cadastrarNovoUsuario(googleUser);                
        }, function (error) {
            Messages(JSON.stringify(error));
        });
}

function cadastrarNovoUsuario(googleUser) {        
    Loading.start();
    if (googleUser != null) {

        var profile = googleUser.getBasicProfile();
        //console.log('ID: ' + googleUser.getAuthResponse().id_token);
        //console.log('Name: ' + profile.getName());
        //console.log('Image URL: ' + profile.getImageUrl());
        //console.log('Email: ' + profile.getEmail());

        let ObjNovoUser = '{';

        ObjNovoUser += '"NOME":"' + profile.getName() + '",';
        ObjNovoUser += '"EMAIL":"' + profile.getEmail() + '",';
        ObjNovoUser += '"URLIMAGEM":"' + profile.getImageUrl() + '"';
        ObjNovoUser += '}';

        $.ajax(
            {
                type: 'POST',
                url: $URL_PRINCIPAL + 'Login/ValidarLoginGoogle',
                data: ObjNovoUser,
                async: true,
                contentType: "application/json; charset=utf-8",
                datatype: 'json',
                success: function (data) {
                    Loading.done();
                    if (data.COCRIACAO) {                        
                        window.location.href = $URL_PRINCIPAL + "Ideia/CoCriador/?idIdeia=" + data.COCRIACAO;
                    } else if (data.AVALIACAO) {
                        window.location.href = $URL_PRINCIPAL + "avaliacao/ideias/?idIdeia=" + data.AVALIACAO;
                    } else {
                        window.location.href = $URL_PRINCIPAL + "Home/";     
                    }
                    
                },
                error: function () {
                    Loading.done();
                    Messages('error');
                }
            });
    } else {
        Loading.done();
        Messages('warning', 'ATENÇÃO', 'Google não retornou informações');
    }               
}

$(document).ready(function () {  
    startApp();
});