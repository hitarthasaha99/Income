window.allowAlphaNumSpace = function (el) {

    // ?? Defensive check
    if (!el || !el.addEventListener) {
        console.error("Invalid element passed:", el);
        return;
    }

    el.addEventListener("beforeinput", function (e) {
        if (e.data && !/^[a-zA-Z0-9 ]+$/.test(e.data)) {
            e.preventDefault();
        }
    });
};

