import http from "./httpService";

export async function getVimeoData(url) {
  let vidObj = { url: url };
  await http
    .get(process.env.REACT_APP_VIMEO_API_URL + url + ".json")
    .then(response => {
      vidObj.duration =
        Math.floor(response.data[0].duration / 60) +
        ":" +
        Math.floor(response.data[0].duration % 60);
      vidObj.thumbnail = response.data[0].thumbnail_small;
      vidObj.title = response.data[0].title;
    });
  return vidObj;
}
