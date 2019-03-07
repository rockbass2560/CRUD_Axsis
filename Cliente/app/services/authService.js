app.service("authService", ["$window", function ($window) {
    var service = this;

    service.parseJwt = function (token) {
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace('-', '+').replace('_', '/');
        return JSON.parse($window.atob(base64));
    };

    service.getNeedLogin = function () {
        return (service.nLogin) ? service.nLogin : false;
    }

    service.activateNeedLogin = function () {
        service.nLogin = true;
    };

    service.saveToken = function (token) {
        $window.localStorage['jwtToken'] = token
    };

    service.logout = function () {
        $window.localStorage.removeItem('jwtToken');
    };

    service.getToken = function () {
        return $window.localStorage['jwtToken'];
    };

    service.isAuthed = function () {
        var token = service.getToken();
        if (token) {
            var params = service.parseJwt(token);
            return Math.round(new Date().getTime() / 1000) <= params.exp;
        } else {
            return false;
        }
    }

}]);