(function () {

    function SendToTranslationController($scope, contentResource, treeService, navigationService, umbRequestHelper, angularHelper, $http) {
        
        var vm = this;

        vm.busy = false;

        vm.languages = [];
        vm.translationRequests = [];

        vm.busy = true;
        vm.error = false;

        umbRequestHelper.resourcePromise(
                    $http.get("/umbraco/backoffice/api/NuTranslation/GetLanguages"),
                    'Failed to request languages')
                    .then(
                    function (data) {
                        vm.languages = _.map(data, function (lang) { lang.selected = false; lang.disabled = false; return lang; });
                        vm.busy = false;
                    },
                    function (err) {
                        vm.success = false;
                        vm.error = err;
                        vm.busy = false;
                    });

        vm.setSource = function (language) {
            angular.forEach(vm.languages, function (item) {
                if (item.isoCode == language) {
                    item.disabled = true;
                    item.selected = false;
                }
                else
                    item.disabled = false;
            });
        };

        vm.toggleAll = function () {
            if (vm.selectedAll) {
                vm.selectedAll = true;
            } else {
                vm.selectedAll = false;
            }
            angular.forEach(vm.languages, function (item) {
                if (item.disabled == false)
                    item.selected = vm.selectedAll;
            });
        };

        vm.sentToTranslation = function () {

            vm.busy = true;
            vm.error = false;

            var args =
            {
                contentId: $scope.currentNode.id,
                sourceLang: vm.langFrom,
                languages: _.map(_.where(vm.languages, { selected: true }), function (lang) { return lang.isoCode; })
            };
            umbRequestHelper.resourcePromise(
                $http.post("/umbraco/backoffice/api/NuTranslation/post", args),
                'Failed to request translation for content')
                .then(function (data) {
                    vm.error = false;
                    vm.success = true;
                    vm.busy = false;
                    vm.translationRequests = data;

                }, function (err) {
                    vm.success = false;
                    vm.error = err;
                    vm.busy = false;
                });

        };

        vm.cancel = function () {
            navigationService.hideDialog();
        };
    }

    angular.module("umbraco").controller("nutranslation.sendToTranslationController", SendToTranslationController);


})();