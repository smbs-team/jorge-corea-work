import axios from 'axios';

export default {
  create: axios.create,
  request: axios.request,
  get: axios.get,
  post: axios.post,
  patch: axios.patch,
  delete: axios.delete,
  cancelToken: axios.CancelToken,
  isCancel: axios.isCancel,
};
