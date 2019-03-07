app.controller("userController",
    function ($scope, $location, NgTableParams, authService, userService) {
        $(".alert").alert("close");

        var isAuthed = authService.isAuthed ? authService.isAuthed() : false;

        if (!isAuthed) {
            $location.path("/");
        }

        $scope.editar = function (usuario) {
            userService.setUsuario(usuario);
            $location.path("/usuarios/editar");
        }

        $scope.goRegister = function () {
            $location.path("/usuarios/registrar");
        }

        $scope.setIdDeshabilitar = function(usuario){
            $scope.usuarioDeshabilitar = usuario;
        }

        function handleDeshabilitar(res) {
            $scope.onDelete = false;
            $(".modal").modal("hide");
            if (res && res.status == 204) {
                $scope.usuarioDeshabilitar.Estatus = false;
            } else {
                $(".alert").alert();
                setTimeout(function () {
                    $(".alert").alert("close");
                }, 5000);
            }
        }

        $scope.deshabilitar = function () {
            $scope.onDelete = true;
            userService.deshabilitar($scope.usuarioDeshabilitar.ID)
                .then(handleDeshabilitar, handleDeshabilitar);
        };

        $scope.tableParams = new NgTableParams(
            {
                count : 5
            },
            {
                counts: [],
                paginationMaxBlocks: 13,
                paginationMinBlocks: 2
            }
        );

        function handleRequest(res) {
            if (res.data) {
                $scope.tableParams.settings({
                    dataset: res.data
                });
                $scope.tableParams.total(res.data.length);
            }
        };

        userService.obtenerTodos().then(handleRequest, handleRequest);    
    }
);