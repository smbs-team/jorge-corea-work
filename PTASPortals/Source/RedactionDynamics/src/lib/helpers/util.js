// Evaluates a condition and return firstComponent if the condition is true, otherwise
// returns secondComponent or undefined if secondComponent is not provided
export const renderIf = (test, firstComponent, secondComponent) =>
  test ? firstComponent : secondComponent || undefined;

// Evaluates if an array has value and any item.
export const arrayNullOrEmpty = array => !(array && array.length > 0);

// Converts a URL byte stream to an file object
export const dataURLtoFile = (dataurl, filename) => {
  var arr = dataurl.split(","),
    mime = arr[0].match(/:(.*?);/)[1],
    bstr = atob(arr[1]),
    n = bstr.length,
    u8arr = new Uint8Array(n);
  while (n--) {
    u8arr[n] = bstr.charCodeAt(n);
  }
  return new File([u8arr], filename, { type: mime });
};

export /**
 * Validates a string for the supported image file types values.
 *
 * @param {string} str
 * @returns {boolean} True if the string contains a valid supported file extension.
 */
const validateFileExtension = str => {
  return str.toLowerCase().match(/\.?(jpeg|jpg|gif|png|tif)$/) != null;
};

export /**
 * Validates is a string is the file name for a TIF | TIFF file.
 *
 * @param {*} str
 * @returns true if file name contains the TIf extension.
 */
const isTifImage = str => {
  return str.toLowerCase().match(/\.?(tif|tiff)$/) != null;
};

export /**
 * Tries to look for a particular query variable on the browser url and return its value.
 *
 * @param {string} variable
 * @returns
 */
const getQueryVariable = variable => {
  var query = window.location.search.substring(1);
  var vars = query.split("&");
  for (var i = 0; i < vars.length; i++) {
    var pair = vars[i].split("=");
    if (pair[0] === variable) {
      return pair[1];
    }
  }
  return false;
};

export /**
 * Extracts the index number from a file name.
 *
 * @param {*} fileName
 * @returns the valid number if the index is found, -1 if not.
 */
const getIndexFromFileName = fileName => {
  // remove the extension from the name.
  const name = fileName
    .split(".")
    .slice(0, -1)
    .join(".");

  // extract the index from the name if any:
  const matches = name.match(/\d+$/);

  if (matches) {
    return parseInt(matches[0]);
  }

  return -1;
};
