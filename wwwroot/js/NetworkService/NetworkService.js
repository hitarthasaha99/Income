
const BASE_ADDRESS_LOGIN = "http://115.124.119.17/APICommon/api/SURVEY/";

async function CallCommonLoginService(network_obj) {
    try {
        network_obj = JSON.parse(network_obj);
        var url = BASE_ADDRESS_LOGIN + network_obj.network_uri;
        var method = network_obj.method_type;
        var data = JSON.parse(network_obj.posted_data);
        var api_response = await axios({
            url: url,
            method: method,
            data: data
        })
        .then(function (response) {
            return response;
        })
        .catch(function (error) {
            return error;
        });
        if (api_response?.id > 0) {
            let obj = {
                message: "Loggedin Successfully!",
                data: JSON.stringify(api_response),
                status: "SUCCESS",
                status_code: "200"
            }
            return JSON.stringify(obj);
        } else {
            return JSON.stringify(api_response?.response.data);
        }
    }
    catch (ex) {
        console.log(ex);
        return null;
    }
}