app.config(function ($routeProvider) {
    $routeProvider.when("/", {
        templateUrl: "app/views/login.html",
        controller: "loginController"
    });

    $routeProvider.when("/login", {
        templateUrl: "app/views/login.html",
        controller: "loginController"
    });

    $routeProvider.when("/usuarios", {
        templateUrl: "app/views/usuarios.html",
        controller: "userController"
    });

    $routeProvider.when("/usuarios/editar", {
        templateUrl : "app/views/usuariosEditar.html",
        controller: "userEditController"
    });

    $routeProvider.when("/usuarios/registrar", {
        templateUrl: "app/views/usuariosInsertar.html",
        controller:"userInsertController"
    });

    $routeProvider.otherwise({
        redirectTo: "/login"
    });
});