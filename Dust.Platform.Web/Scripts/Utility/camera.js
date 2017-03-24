(function () {
    var camViewer = {
        started: false,
        setuped: false,
        setup: function (obj) {
            if (obj == null) return;
            camViewer.target = obj;
            camViewer.setuped = true;
        },
        preview: function (e) {
            if (!camViewer.setuped) return;
            camViewer.started = camViewer.target.Preview();
            e.target.innerHTML = camViewer.started ? "停止预览" : "开始预览";
        },
        tryLogin: function (parString) {
            camViewer.target.TryLogin(parString);
        },
        up: function () {
            if (!camViewer.setuped || !camViewer.started) return;
            camViewer.target.PlatformControl('TiltUp');
        },
        down: function () {
            if (!camViewer.setuped || !camViewer.started) return;
            camViewer.target.PlatformControl('TiltDown');
        },
        left: function () {
            if (!camViewer.setuped || !camViewer.started) return;
            camViewer.target.PlatformControl('PanLeft');
        },
        right: function () {
            if (!camViewer.setuped || !camViewer.started) return;
            camViewer.target.PlatformControl('PanRight');
        },
        near: function () {
            if (!camViewer.setuped || !camViewer.started) return;
            camViewer.target.PlatformControl('FocusNear');
        },
        far: function () {
            if (!camViewer.setuped || !camViewer.started) return;
            camViewer.target.PlatformControl('FocusFar');
        },
        upStop: function () {
            if (!camViewer.setuped || !camViewer.started) return;
            camViewer.target.PlatformControl('TiltUp', true);
        },
        downStop: function () {
            if (!camViewer.setuped || !camViewer.started) return;
            camViewer.target.PlatformControl('TiltDown', true);
        },
        leftStop: function () {
            if (!camViewer.setuped || !camViewer.started) return;
            camViewer.target.PlatformControl('PanLeft', true);
        },
        rightStop: function () {
            if (!camViewer.setuped || !camViewer.started) return;
            camViewer.target.PlatformControl('PanRight', true);
        },
        nearStop: function () {
            if (!camViewer.setuped || !camViewer.started) return;
            camViewer.target.PlatformControl('FocusNear', true);
        },
        farStop: function () {
            if (!camViewer.setuped || !camViewer.started) return;
            camViewer.target.PlatformControl('FocusFar', true);
        }
    }
    window.camViewer = camViewer;

})()