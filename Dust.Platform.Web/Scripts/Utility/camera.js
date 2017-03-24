(function () {
    var camViewer = {
        started: false,
        setup: function(obj) {
            camViewer.target = obj;
        },
        preview: function (e) {
            camViewer.started = camViewer.target.Preview();
            e.target.innerHTML = camViewer.started ? "停止预览" : "开始预览";
        },
        tryLogin: function (parString) {
            camViewer.target.TryLogin(parString);
        },
        up: function () {
            if (!camViewer.started) return;
            camViewer.target.PlatformControl('TiltUp');
        },
        down: function () {
            if (!camViewer.started) return;
            camViewer.target.PlatformControl('TiltDown');
        },
        left: function () {
            if (!camViewer.started) return;
            camViewer.target.PlatformControl('PanLeft');
        },
        right: function () {
        if (!camViewer.started) return;
            camViewer.target.PlatformControl('PanRight');
        },
        near: function () {
            if (!camViewer.started) return;
            camViewer.target.PlatformControl('FocusNear');
        },
        far: function () {
            if (!camViewer.started) return;
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