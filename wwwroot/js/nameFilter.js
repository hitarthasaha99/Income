window.attachAlphaFilter = (el) => {
    el.addEventListener('beforeinput', (e) => {
        if (e.data && !/^[a-zA-Z\s]*$/.test(e.data)) {
            e.preventDefault();
        }
    });
};