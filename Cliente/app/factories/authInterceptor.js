app.factory("authInterceptor", function (API, authService) {
    return {
        request: function (config) {
            var token = authService.getToken();
            if (config.url.indexOf("api/auth/login") < 0 && token) {
                config.headers.Authorization = 'Bearer ' + token;
            }
            return config;
        },

        response: function (res) {
            if (res.config.url.indexOf(API) === 0 && res.data.token) {
                authService.saveToken(res.data.token);
            }

            return res;
        },
    }
}).config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptor');
    });