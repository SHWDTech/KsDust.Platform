(function () {
    var camViewer = {
        started: false,
        setup: function(obj) {
            camViewer.target = obj;
        },
        preview: function (e) {
            if (camViewer.started) {
                camViewer.target.StopPreview();
            } else {
                camViewer.target.StartPreview();
            }
            camViewer.started = !camViewer.target.PreviewStarted;
            e.target.innerHTML = camViewer.started ? "停止预览" : "开始预览";
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