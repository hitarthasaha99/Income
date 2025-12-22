(function () {

    // Matches most emoji (Unicode extended pictographs)
    const emojiRegex = /\p{Extended_Pictographic}/u;

    document.addEventListener("beforeinput", function (e) {

        // We only care about text insertion
        if (!e.data) return;

        // Block emoji input
        if (emojiRegex.test(e.data)) {
            e.preventDefault();
        }
    });

})();
