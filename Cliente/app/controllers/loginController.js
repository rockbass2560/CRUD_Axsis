app.controller("loginController", 
    function ($scope, $location, authService, userService, userFactory) {

        $scope.usuario = userFactory.getDefaultUser();

        function handleRequest(res) {
            $scope.noAutorizado = false;
            $scope.errorSistema = false;

            if (res.status == 401) { //No autorizado
                $scope.noAutorizado = true;
            } else if (res.status == 200) {
                //Login autorizado, token guardado
                var token = res.data ? res.data.token : null;
                if (token) {
                    $location.path("/usuarios");
                }
            } else {
                $scope.errorSistema = true;
            }

            $scope.doLogin = false;
        }

        $scope.login = function (loginForm) {
            if (loginForm.$valid) {
                $scope.doLogin = true;
                userService.login(this.usuario.nombreUsuario, this.usuario.contrasena)
                    .then(handleRequest, handleRequest);
            }
        }

        $scope.goRegistro = function () {
            authService.activateNeedLogin();
            $location.path('/usuarios/registrar');
        };

        $scope.isAuthed = authService.isAuthed ? authService.isAuthed() : false;

        //Si esta autenticado, lo mandamos a pantalla principal
        if ($scope.isAuthed)
            $location.path("/usuarios");
    }
);