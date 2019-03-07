app.service("userService", function ($http, API) {
    var service = this;
    
    service.login = function (nombreUsuario, contrasena) {
        return $http.post(API + '/auth/login', {
            nombreUsuario: nombreUsuario,
            contrasena: contrasena
        });
    };

    service.actualizar = function (usuario) {
        return $http.put(API + "/usuarios", usuario);
    }

    service.registrar = function (usuario) {
        return $http.post(API + '/usuarios', usuario);
    };

    service.deshabilitar = function (id) {
        return $http.put(API + "/usuarios/" + id);
    }

    service.obtenerTodos = function (usuario) {
        return $http.get(API + "/usuarios");
    }

    service.setUsuario = function (usuario) {
        sessionStorage.setItem("usuario", JSON.stringify(usuario));
    }

    service.getUsuario = function (usuario) {
        return JSON.parse(sessionStorage.getItem("usuario"));
    }

});