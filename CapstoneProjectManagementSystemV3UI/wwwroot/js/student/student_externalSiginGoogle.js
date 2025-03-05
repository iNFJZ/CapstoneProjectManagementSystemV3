var googleUser = {};
var startApp = function () {
    gapi.load('auth2', function () {
        // Retrieve the singleton for the GoogleAuth library and set up the client.
        auth2 = gapi.auth2.init({
            client_id: '108406507970-iek416cmdtu5gkg0e7i5lo9sse8vkejr.apps.googleusercontent.com',
            cookiepolicy: '/google-response',
            // Request scopes in addition to 'profile' and 'email'
            scope: 'profile email openid'
        });
        attachSignin(document.getElementById('signinGg'));
    });
};
function attachSignin(element) {

    auth2.attachClickHandler(element, {},
        function (googleUser) {
            var campus = document.getElementById('campus').value;
            var id_token = googleUser.getAuthResponse().id_token;
            //console.log(campus, '---------', campus);
            window.location.href = "/ExternalSignIn/SignInGoogle?returnUrl=@ViewBag.ReturnUrl&campus=" + campus +"&token=" + id_token;
        }, function (error) {
            console.log(JSON.stringify(error, undefined, 2));
        });
}