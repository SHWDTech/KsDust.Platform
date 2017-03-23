(function () {
    var camViewer = {
        started: false,
        setup: function(obj) {
            camViewer.target = obj;
        },
        preview: function (e) {
            if (camViewer.started) {
                e.target.innerHTML = "开始预览";
                camViewer.target.StopPreview();
            } else {
                e.target.innerHTML = "停止预览";
                camViewer.target.StartPreview();
            }
            camViewer.started = !camViewer.started;
        },
        tryLogin: function (parString) {
            camViewer.target.TryLogin(parString);
        },
        up: function() {
            camViewer.target.PlatformControl('TiltUp');
        },
        down: function () {
            camViewer.target.PlatformControl('TiltDown');
        },
        left: function () {
            camViewer.target.PlatformControl('PanLeft');
        },
        right: function () {
            camViewer.target.PlatformControl('PanRight');
        },
        near: function () {
            camViewer.target.PlatformControl('FocusNear');
        },
        far: function () {
            camViewer.target.PlatformControl('FocusFar');
        },
        upStop: function () {
            camViewer.target.PlatformControl('TiltUp', true);
        },
        downStop: function () {
            camViewer.target.PlatformControl('TiltDown', true);
        },
        leftStop: function () {
            camViewer.target.PlatformControl('PanLeft', true);
        },
        rightStop: function () {
            camViewer.target.PlatformControl('PanRight', true);
        },
        nearStop: function () {
            camViewer.target.PlatformControl('FocusNear', true);
        },
        farStop: function () {
            camViewer.target.PlatformControl('FocusFar', true);
        }
    }
    window.camViewer = camViewer;

})()