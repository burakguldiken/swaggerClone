import Vue from "vue";
import axios from "axios";
import VueAxios from "vue-axios";
import { objectToFormData } from 'object-to-formdata'
import { CB_API_URL } from "@/common/config";
import store from '@/store';
import { IS_LOADING } from "@/store/actions.type";

const ApiService = {
    init() {
        Vue.use(VueAxios, axios);
        Vue.axios.defaults.baseURL = CB_API_URL;
    },
    
    post(resource, params) {
        delete Vue.axios.defaults.headers.post['Content-Type']
        return new Promise((resolve, reject) => {
          store.dispatch(IS_LOADING);
          Vue.axios.post(`${resource}`, params).then((response) => {
            resolve(response.data);
          }).catch((err) => {
            if (!err.response) {
              const data = {
                errorMessage: 'Api Bağlantı Hatası'
              };
              reject(data);
            } else {
              const statusCode = err.response.status;
              if (statusCode === 401) {
                reject(err.response.data)
              } else if (statusCode === 404) {
                reject(err.response.data);
              }
            }
          }).finally(() => {
            store.dispatch(IS_LOADING)
          })
        })
      },

    postFile(resource, params) {
        return new Promise((resolve, reject) => {
            Vue.axios.defaults.headers.post['Content-Type'] = "multipart/form-data"
            let formData = new FormData();
            this.getformData(formData, params, null);
            Vue.axios.post(`${resource}`, formData).then((response) => {
                resolve(response.data);
            }).catch((err) => {
                if (!err.response) {
                    const data = {
                        errorMessage: 'Api Bağlantı Hatası',
                    };
                    reject(data);
                }
                const statusCode = err.response.status;
                if (statusCode === 401 && !err.response.data.errorMessage) {
                    console.log("401");
                }
                else if (statusCode === 404) {
                    reject(err.response.data);
                }
            }).finally(() => {
            })
        })
    },

    get(resource) {
        delete Vue.axios.defaults.headers.post['Content-Type']
        return new Promise((resolve, reject) => {
            store.dispatch(IS_LOADING)
            Vue.axios.get(`${resource}`).then((response) => {
                resolve(response.data);
            }).catch((err) => {
                if (!err.response) {
                    const data = {
                        errorMessage: 'Api Bağlantı Hatası',
                    };
                    reject(data);
                }
                const statusCode = err.response.status;
                if (statusCode === 401 && !err.response.data.errorMessage) {
                    console.log("401");
                }
                else if (statusCode === 404) {
                    reject(err.response.data);
                }
            }).finally(() => {
                store.dispatch(IS_LOADING)
            })
        })
    },

    put(resource, params) {
        console.log(params);
        return new Promise((resolve, reject) => {
            Vue.axios.defaults.headers.post['Content-Type'] = 'application/json';
            Vue.axios.put(`${resource}`, params).then((response) => {
                resolve(response.data);
            }).catch((err) => {
                if (!err.response) {
                    const data = {
                        errorMessage: 'Api Bağlantı Hatası',
                    };
                    reject(data);
                }
                const statusCode = err.response.status;
                if (statusCode === 401 && !err.response.data.errorMessage) {
                    console.log("401");
                }
                else if (statusCode === 404) {
                    reject(err.response.data);
                }
            }).finally(() => {
            })
        })
    },

    putFile(resource, params) {
        return new Promise((resolve, reject) => {
            Vue.axios.defaults.headers.post['Content-Type'] = 'multipart/form-data';
            let formData = new FormData();
            formData = objectToFormData(params);
            Vue.axios.put(`${resource}`, formData).then((response) => {
                resolve(response.data);
            }).catch((err) => {
                if (!err.response) {
                    const data = {
                        errorMessage: 'Api Bağlantı Hatası',
                    };
                    reject(data);
                }
                const statusCode = err.response.status;
                if (statusCode === 401 && !err.response.data.errorMessage) {
                    console.log("401");
                }
                else if (statusCode === 404) {
                    reject(err.response.data);
                }
            }).finally(() => {
            })
        })
    },

    delete(resource, params) {
        delete Vue.axios.defaults.headers.post['Content-Type']
        Vue.axios.defaults.headers.post['Content-Type'] = 'application/json';
        return new Promise((resolve, reject) => {
            Vue.axios.delete(`${resource}`, params).then((response) => {
                resolve(response.data);
            }).catch((err) => {
                if (!err.response) {
                    const data = {
                        errorMessage: 'Api Bağlantı Hatası',
                    };
                    reject(data);
                }
                const statusCode = err.response.status;
                if (statusCode === 401 && !err.response.data.errorMessage) {
                    console.log("401");
                }
                else if (statusCode === 404) {
                    reject(err.response.data);
                }
            }).finally(() => {
            })
        })
    },

    getformData(formData, data, parentKey) {
        if (data && typeof data === 'object' && !(data instanceof Date) && !(data instanceof File)) {
            Object.keys(data).forEach(key => {
                if (data[key] instanceof File) {
                    formData.append(key, data[key])
                } else {
                    this.getformData(formData, data[key], parentKey ? `${parentKey}[${key}]` : key);
                }
            });
        } else {
            const value = data == null ? '' : data;
            formData.append(parentKey, value);
        }
    }
};

export default ApiService;