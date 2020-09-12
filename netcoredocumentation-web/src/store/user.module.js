import ApiService from '@/common/api.service'
import {
    GET_DOCUMENTATION,
    GET_EXECUTE
} from '@/store/actions.type'
import {
    SET_DOCUMENTATION,
    SET_EXECUTE
} from '@/store/mutations.type'

const state = {
    documentKey: "",
    documentValueInfo: [],
    executeResult: {}
}

const getters = {
    getDocumentKeyInfo: state => {
        return state.documentKey;
    },
    getDocumentValueInfo: state => {
        return state.documentValueInfo;
    },
    getExecuteInfo: state => {
        return state.executeResult;
    }
}


const actions = {
    [GET_DOCUMENTATION](context) {
        return new Promise((resolve, reject) => {
            ApiService.get('/user/documentinfo').then((payload) => {
                context.commit(SET_DOCUMENTATION, payload.data);
                resolve(payload)
            }).catch((err) => {
                reject(err)
            })
        })
    },
    [GET_EXECUTE](context, payload) {
        var methodType = payload.item.ActionName;
        if (methodType == "Get") {
            var getMethodName = payload.item.ActionDescription;
            return new Promise((resolve, reject) => {
                ApiService.get(`/user/${getMethodName}`).then((payload) => {
                    context.commit(SET_EXECUTE, payload);
                    resolve(payload)
                }).catch((err) => {
                    reject(err)
                })
            })
        }
        else if (methodType == "Post") {
            var postMethodName = payload.item.ActionDescription;
            return new Promise((resolve, reject) => {
                ApiService.post(`user/${postMethodName}`, payload.values).then((payload) => {
                    context.commit(SET_EXECUTE, payload);
                    resolve(payload);
                }).catch((err) => {
                    reject(err)
                })
            });
        }
        else if (methodType == "Put") {
            var putMethodName = payload.item.ActionDescription.substring(0,payload.item.ActionDescription.indexOf('/'));
            return new Promise((resolve, reject) => {
                ApiService.post(`user/${putMethodName}/${payload.values}`, payload.values).then((payload) => {
                    context.commit(SET_EXECUTE, payload);
                    resolve(payload);
                }).catch((err) => {
                    reject(err)
                })
            });
        }
        else if (methodType == "Delete") {
            return new Promise((resolve, reject) => {
                var deleteMethodName = payload.item.ActionDescription.substring(0,payload.item.ActionDescription.indexOf('/'));
                ApiService.delete(`/user/${deleteMethodName}/${payload.values}`).then((payload) => {
                    context.commit(SET_EXECUTE, payload);
                    resolve(payload)
                }).catch((err) => {
                    reject(err)
                })
            })
        }
    },
}

const mutations = {
    [SET_DOCUMENTATION](state, payload) {
        state.documentKey = payload.key;
        state.documentValueInfo = payload.value;
    },
    [SET_EXECUTE](state, payload) {
        console.log(payload);
        state.executeResult = payload;
    }
}

export default {
    state,
    getters,
    actions,
    mutations
}