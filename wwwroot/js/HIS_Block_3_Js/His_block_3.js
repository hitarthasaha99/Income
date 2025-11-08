window.validationHelpers = {
    validateName: function () {
        let name = document.getElementById('nameInput').value;
        // Regex: letters (upper and lower) and spaces only
        let regex = /^[A-Za-z\s]+$/;
        if (!regex.test(name)) {
            alert('Name must contain only letters and spaces (no numbers or symbols).');
            return false;
        }
        return true;
    },
    validateAge: function () {
        let ageVal = document.getElementById('ageInput').value;
        let age = Number(ageVal);
        if (!/^\d+$/.test(ageVal) || age < 0 || age > 150) {
            alert('Age must be a number between 0 and 150.');
            return false;
        }
        return true;
    }
};
