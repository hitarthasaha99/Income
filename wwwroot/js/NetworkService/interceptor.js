const BASE_ADDRESS_SURVEY = "http://115.124.119.17/APItravel/api/SURVEY/";

const axiosInstance = axios.create({
    baseURL: BASE_ADDRESS_SURVEY
});

var count = 0;

axiosInstance.interceptors.request.use(
    function (config) {
        // Do something before request is sent
        config.headers['Authorization'] = 'Bearer ' + localStorage.getItem('token');
        config.headers['Content-Type'] = 'application/json';
        config.headers['Accept'] = 'application/json';
        count++;
        //document.getElementById('body_element').classList.add('loader');
        return config;
    },
    function (error) {
        count--;
        if (count <= 0) {
           // document.getElementById('body_element').classList.remove('loader');
        }
        return Promise.reject(error);
    }
);
axiosInstance.interceptors.response.use(
    function (response) {
        // Do something with response data
        count--;
        if (count <= 0) {
            //document.getElementById('body_element').classList.remove('loader');
        }
        return response.data;
    },
    function (error) {
        // Do something with response error
        count--;
        if (count <= 0) {
           // document.getElementById('body_element').classList.remove('loader');
        }
        //if (error.response.status === 401) {
        //    localStorage.removeItem('token');
        //    window.location.href = '/login.html';
        //}
        return Promise.reject(error);
    }
)