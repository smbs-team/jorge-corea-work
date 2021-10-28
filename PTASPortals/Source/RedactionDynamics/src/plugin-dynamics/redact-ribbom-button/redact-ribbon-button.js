const toolurl = "https://ptasredactiondynamics-sandbox-web.azurewebsites.net/";
function OpenTool(documentid) {
  /* authContext.acquireToken("404b9b8b-5315-4ef3-859d-37352a12b2da",  function (errorDesc, token, error) {
        if (error) { //acquire token failure
            if (config.popUp) {
                // If using popup flows
                authenticationContext.acquireTokenPopup(webApiConfig.resourceId, null, null,  function (errorDesc, token, error) {});
            }
            else {
            // In this case the callback passed in the Authentication request constructor will be called.
                authenticationContext.acquireTokenRedirect(webApiConfig.resourceId, null, null);
            }
        }
        else {
            //acquired token successfully
            window.open(`${toolurl}?id=${documentid}&token=${token}`, '_blank');
        }
    });*/
  window.open(`${toolurl}?id=${documentid}&token=notyet`, "_blank");
}

async function OpenToolFromItem(item) {
  var selectedItem = item[0];
  if (selectedItem.TypeName == "ptas_fileattachmentmetadata") {
    var record = await Xrm.WebApi.retrieveRecord(
      selectedItem.TypeName,
      selectedItem.Id
    );
    if (record.ptas_isilinx) {
      OpenTool(record.ptas_icsdocumentid);
    } else {
      alert("The selected document is not yet in iLinx, and can't be edited.");
    }
  } else {
    OpenTool(selectedItem.Name);
  }
}

async function IsIlinx(item) {
  var selectedItem = item[0];
  if (selectedItem.TypeName == "ptas_fileattachmentmetadata") {
    var record = await Xrm.WebApi.retrieveRecord(
      selectedItem.TypeName,
      selectedItem.Id
    );
    if (record.ptas_isilinx) {
      return true;
    } else {
      return false;
    }
  } else {
    return true;
  }
}
