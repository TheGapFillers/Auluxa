// Desactivate logic around the explore button and remove api key
$('#explore').off();
$("#input_apiKey").hide();
$('#input_username').remove();
$('#input_password').remove();

if (!window.swaggerUi.api.clientAuthorizations.authz.AuthorizationHeader) { // not authorized
// insert username and password instead of api key
    var usernameAndPasswordUi =
        '<div class="input"><input placeholder="username" id="input_username" name="username" type="text" size="10"></div>' +
        '<div class="input"><input placeholder="password" id="input_password" name="password" type="password" size="10"></div>';
    $(usernameAndPasswordUi).insertBefore('#api_selector div.input:last-child');
} else { // authorized
    $('#input_username').remove();
    $('#input_password').remove();
    $('#explore').remove();
}

// Activate onClick on Explore
$('#explore').click(function () {
    var username = $('#input_username').val();
    var password = $('#input_password').val();

    if (username && username.trim() !== "" && password && password.trim() !== "") {
        // send request to auth service
        $.ajax({
            url: window.swashbuckleConfig.oAuth2Realm + 'token', // the realm is set as the auth service realm (could be better)
            type: "post",
            contenttype: 'x-www-form-urlencoded',
            data: "grant_type=password" +
                "&client_id=" + window.swashbuckleConfig.oAuth2ClientId +
                "&client_secret=" + window.swashbuckleConfig.oAuth2ClientSecret +
                "&username=" + username +
                "&password=" + password,
            success: function (response) {
                var bearerToken = 'bearer ' + response.access_token;

                // Add the authorization header with the bearer token for every call from now.
                window.swaggerUi.api.clientAuthorizations.add('AuthorizationHeader', new window.SwaggerClient.ApiKeyAuthorization('Authorization', bearerToken, 'header'));
                window.swaggerUi.api.clientAuthorizations.remove("api_key");
                alert("Login successful!");

                // Remove username, password and explore
                $('#input_username').remove();
                $('#input_password').remove();
                $('#explore').remove();

                // Add loggedAs header
                var loggedAs = '<div class="input"><a id="loggedAs" href="#" data-sw-translate>Logged as ' + username + '</a></div>';
                $(loggedAs).insertBefore('#api_selector div.input:last-child');
            },
            error: function (xhr, ajaxoptions, thrownerror) {
                alert("Login failed!");
            }
        });
    }
}
)();