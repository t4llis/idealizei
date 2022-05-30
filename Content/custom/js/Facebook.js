$(document).ready(function () {

    
    function callbackMudancaStatus(response){

        if (response.status === 'connected') {

            testAPI();

            FB.logout(function (response) {});

        } else if (response.status === 'not_authorized') {
            
        } else {
            
        }
    }

    window.fbAsyncInit = function () {
        FB.init({
            appId: '181770913961477',
            cookie: false,
            xfbml: true,
            version: 'v11.0'
        });

        FB.AppEvents.logPageView();

        FB.getLoginStatus(function (response) {
            callbackMudancaStatus(response);
        });
    };

    //Codigo do facebook para inicialização do
    //JavaScript SDK
    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

    function testAPI() {
        Loading.start();
        FB.api(
            '/me',            
            { "fields": "name,email,picture" },
            function (response) {
                if (response.name != "" && response.name != null) {

                    let ObjNovoUser = '{';

                    ObjNovoUser += '"NOME":"' + response.name + '",';
                    ObjNovoUser += '"EMAIL":"' + response.email + '",';
                    ObjNovoUser += '"URLIMAGEM":"' + response.picture.data.url + '"';
                    ObjNovoUser += '}';

                    $.ajax(
                        {
                            type: 'POST',
                            url: $URL_PRINCIPAL + 'Login/ValidarLoginFacebook',
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

                                FB.logout(function (response) { });

                            },
                            error: function () {
                                Loading.done();
                                Messages('error');
                            }
                        });
                } else {
                    Loading.done();
                }
        });                           

        Loading.done();
    }

    $('.btn_entrar_facebook').on('click', function () {
        // function login
        FB.login(function (response) {
            callbackMudancaStatus(response);
        }, { scope: 'email, public_profile' });        
    });   

});