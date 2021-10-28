import { FormattedMessage } from "react-intl";
import React from "react";

export const whatShouldObscure = (
  <FormattedMessage
    id="imageCrossout_whatShouldObscure"
    defaultMessage="    
    It's important to obscure Social Security and account numbers on your documents.{br}{br}
    You can obscure them with our online tool as you upload your documents.{br}{br}
    Or, before you upload, copy your documents and obscure this information using paper or tape, a felt pen,
    or an online image-editing program. Then, upload the redacted copies."
    values={{ br: <br /> }}
  />
);

export const nextPage = (
  <FormattedMessage id="imageCrossout_nextPage" defaultMessage="Next Page" />
);

export const continueButton = (
  <FormattedMessage
    id="imageCrossout_continue"
    defaultMessage="Close and Save"
  />
);
