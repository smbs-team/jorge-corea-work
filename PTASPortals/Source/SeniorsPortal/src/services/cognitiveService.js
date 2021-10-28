import http from './httpService';

let baseURL = process.env.REACT_APP_OCR_API_URL;

let cancelPostImageOcr;
let cancelImageAnalysis;

async function postImageOcr(image) {
  let IEPost = async (url = '', data = {}) => {
    // Default options are marked with *
    const response = await fetch(url + '?detectOrientation=true&language=unk', {
      method: 'POST', // *GET, POST, PUT, DELETE, etc.
      mode: 'cors', // no-cors, *cors, same-origin
      cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
      credentials: 'same-origin', // include, *same-origin, omit
      headers: {
        'Content-Type': 'application/octet-stream',
        'Ocp-Apim-Subscription-Key': process.env.REACT_APP_OCR_SUB_KEY,
      },
      redirect: 'follow', // manual, *follow, error
      referrerPolicy: 'no-referrer', // no-referrer, *client
      body: data, // body data type must match "Content-Type" header
    });
    return await response; // parses JSON response into native JavaScript objects
  };
  try {
    let data2 = '';
    await IEPost(baseURL + '/ocr', image).then(async data => {
      await data.json().then(async data3 => {
        console.log('response JSON data 3-------', data3);
        data2 = data3;
      }); // JSON data parsed by `response.json()` call
    });

    return data2;
  } catch (error) {
    alert(error);
  }
}

async function postImageOcrRemote(url) {
  const instance = http.create({
    url: '/ocr',
    method: 'post',
    baseURL: baseURL,
    headers: {
      'Content-Type': 'application/json',
      'Ocp-Apim-Subscription-Key': process.env.REACT_APP_OCR_SUB_KEY,
    },
    params: { language: 'unk', detectOrientation: 'true' },
    data: '{"url": ' + '"' + url + '"}',
    cancelToken: new http.cancelToken(function executor(c) {
      cancelPostImageOcr = c;
    }),
  });

  try {
    const { data } = await instance.request();
    return data;
  } catch (error) {}
}

async function postImageAnalysis(image) {
  let resultData;
  let IEPost = async (url = '', data = {}) => {
    var params = [
      ['visualFeatures', 'Adult'],
      ['language', 'en'],
    ];

    // Default options are marked with *
    const response = await fetch(url + '?visualFeatures=Adult&language=en', {
      method: 'POST', // *GET, POST, PUT, DELETE, etc.
      mode: 'cors', // no-cors, *cors, same-origin
      cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
      credentials: 'same-origin', // include, *same-origin, omit
      headers: {
        'Content-Type': 'application/octet-stream',
        'Ocp-Apim-Subscription-Key': process.env.REACT_APP_OCR_SUB_KEY,
      },
      redirect: 'follow', // manual, *follow, error
      referrerPolicy: 'no-referrer', // no-referrer, *client
      body: data, // body data type must match "Content-Type" header
    });

    await response; // parses JSON response into native JavaScript objects

    return response;
  };

  try {
    let data2 = '';
    await IEPost(baseURL + '/analyze', image).then(async data => {
      await data.json().then(async data3 => {
        console.log('response JSON data 3-------', data3);
        data2 = data3;
      }); // JSON data parsed by `response.json()` call
    });
    console.log('CognitiveService data 2 ----------', data2);
    return data2;
  } catch (error) {}
}

async function postImageAnalysisRemote(url) {
  const instance = http.create({
    url: '/analyze',
    method: 'post',
    baseURL: baseURL,
    headers: {
      'Content-Type': 'application/json',
      'Ocp-Apim-Subscription-Key': process.env.REACT_APP_OCR_SUB_KEY,
    },
    params: {
      visualFeatures: 'Adult',
      language: 'en',
    },
    data: '{"url": ' + '"' + url + '"}',
    cancelToken: new http.cancelToken(function executor(c) {
      cancelImageAnalysis = c;
    }),
  });

  try {
    const { data } = await instance.request();
    return data;
  } catch (error) {
    //if (http.isCancel(error)) console.log('Analysis Cancelled');
    //console.log(error);
  }
}

export async function isValidImage(image) {
  const [analysis, ocr] = await Promise.all([
    postImageAnalysis(image),
    postImageOcr(image),
  ]);

  if (
    analysis?.adult?.isAdultContent ||
    analysis?.adult?.isRacyContent ||
    (ocr?.regions && !ocr.regions.length)
  ) {
    return false;
  } else return true;
}

export async function isValidImageRemote(url) {
  const [analysis, ocr] = await Promise.all([
    postImageAnalysisRemote(url),
    postImageOcrRemote(url),
  ]);

  if (
    analysis.adult.isAdultContent ||
    analysis.adult.isRacyContent ||
    !ocr.regions.length
  ) {
    return false;
  } else return true;
}

export function cancelRequests() {
  cancelImageAnalysis();
  cancelPostImageOcr();
}
