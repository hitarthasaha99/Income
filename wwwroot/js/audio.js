window.playAudio = (id) => {
    const el = document.getElementById(id);
    if (el) {
        el.currentTime = 0; // restart if already playing
        el.play();
    }
};