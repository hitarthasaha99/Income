async function __ReturnValidationStatus(field_type, field_value, reg_exp = "", min, max) {
    min = !!min ? parseInt(min) : 0;
    max = !!max ? parseInt(max) : 0;
    switch (field_type) {
        case "SELECT":
        case "DEFAULT":
            if (!field_value) {
                return true;
            } else {
                return false;
            }
        case "REG_EXP":
            if (!field_value) {
                return true;
            }
            if (!!field_value) {
                var req_reg_exp = new RegExp(reg_exp.slice(1, -1));
                var status = req_reg_exp.test(field_value);
                return !status ? true : false;
            }
            return false;
        case "REG_EXP_NOT_REQUIRED":
            if (!!field_value) {
                var req_reg_exp = new RegExp(reg_exp.slice(1, -1));
                var status = req_reg_exp.test(field_value);
                return !status ? true : false;
            }
            return false;
        case "MIN_MAX":
            let min_max_int_val = parseInt(field_value);
            let min_max_val = !!min_max_int_val ? isNaN(min_max_int_val) ? 0 : min_max_int_val : 0;
            if (!min_max_val) {
                return true;
            }
            if (!!min_max_val) {
                if (min_max_val < min || min_max_val > max) {
                    return true;
                }
            }
            return false;
        case "ONLY_MIN":
            let min_int_val = parseInt(field_value);
            let min_val = !!min_int_val ? isNaN(min_int_val) ? 0 : min_int_val : 0;
            if (!min_val) {
                return true;
            }
            if (!!min_val) {
                if (min_val < min) {
                    return true;
                }
            }
            return false;
        case "ONLY_MAX":
            let max_int_val = parseInt(field_value);
            let max_val = !!max_int_val ? isNaN(max_int_val) ? 0 : max_int_val : 0;
            if (!max_val) {
                return true;
            }
            if (!!max_val) {
                if (max_val > max) {
                    return true;
                }
            }
            return false;
        default:
            return false;
    }
}

async function __FieldValidate(required_fields, fields) {
    required_fields = JSON.parse(required_fields);
    let validation_status = 1;
    fields = JSON.parse(fields);
    for (let i = 0; i < required_fields.length; i++) {
        var field = required_fields[i];
        var field_type = field.field_type;
        var field_value = required_fields[i].is_stringify_val ? fields[field.field_name]?.toString() : fields[field.field_name];
        var reg_exp = field.reg_exp;
        if (field_type == "SELECT" || field_type == "DEFAULT") {
            if (!field_value) {
                validation_status = 0;
                break;
            }
        } else if (field_type == "REG_EXP") {
            if (!field_value) {
                validation_status = 0;
                break;
            } else {
                var status = await __ReturnValidationStatus(field_type, field_value, reg_exp);
                if (status) {
                    validation_status = 0;
                    break;
                }
            }
        } else if (field_type == "REG_EXP_NOT_REQUIRED") {
            if (!!field_value) {
                var status = await __ReturnValidationStatus(field_type, field_value, reg_exp);
                if (status) {
                    validation_status = 0;
                    break;
                }
            }
        } else if (field_type == "MIN_MAX") {
            if (!field_value) {
                validation_status = 0;
                break;
            } else {
                let int_val = parseInt(field_value);
                let val = !!int_val ? isNaN(int_val) ? 0 : int_val : 0;
                var status = await __ReturnValidationStatus(field_type, val, "", required_fields[i].min_val, required_fields[i].max_val);
                if (status) {
                    validation_status = 0;
                    break;
                }
            }
        } else if (field_type == "ONLY_MIN") {
            if (!field_value) {
                validation_status = 0;
                break;
            } else {
                let int_val = parseInt(field_value);
                let val = !!int_val ? isNaN(int_val) ? 0 : int_val : 0;
                var status = await __ReturnValidationStatus(field_type, val, "", required_fields[i].min_val, required_fields[i].max_val);
                if (status) {
                    validation_status = 0;
                    break;
                }
            }
        } else if (field_type == "ONLY_MAX") {
            if (!field_value) {
                validation_status = 0;
                break;;
            } else {
                let int_val = parseInt(field_value);
                let val = !!int_val ? isNaN(int_val) ? 0 : int_val : 0;
                var status = await __ReturnValidationStatus(field_type, val, "", required_fields[i].min_val, required_fields[i].max_val);
                if (status) {
                    validation_status = 0;
                    break;
                }
            }
        }
    }
    return validation_status;
}

async function __handleGoBack() {
    window.history.back();
}

async function __disableFieldsAndButtons() {
    // Disable elements with class 'is_sso'
    var btns = document.getElementsByClassName('is_sso');
    var actions = document.getElementsByClassName('action');
    if (btns && btns.length > 0) {
        for (let i = 0; i < btns.length; i++) {
            btns[i].classList.add("disabled");
            btns[i].disabled = true;
        }
    }
    if (actions && actions.length > 0) {
        for (let i = 0; i < actions.length; i++) {
            btns[i].classList.add("d-none");
        }
    }
    return;
}

function uncheckRadioById(radioId) {
    var radio = document.getElementById(radioId);
    if (radio) {
        radio.checked = false;
    }
}

async function downloadFile (fileName, base64Data) {
    const link = document.createElement('a');
    link.download = fileName;
    const base_64 = "data:application/vnd.openxmlformats-officedocument.wordprocessingml.document;base64," + base64Data;
    return base_64;
}

