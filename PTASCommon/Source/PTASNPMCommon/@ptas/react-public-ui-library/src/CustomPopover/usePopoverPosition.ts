import { PopoverOrigin } from "@material-ui/core";
import { useState, useEffect, useRef } from "react";
import ReactDOM from "react-dom";

export const usePopoverPosition = (
  anchorOriginInit: PopoverOrigin | undefined,
  transformOriginInit: PopoverOrigin | undefined,
  anchorEl: HTMLElement | null | undefined,
  popoverEl: HTMLElement | null
) => {
  const [anchorOrigin, setAnchorOrigin] = useState<PopoverOrigin>();
  const [transformOrigin, setTransformOrigin] = useState<PopoverOrigin>();
  const tailRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    setAnchorOrigin(anchorOriginInit);
  }, [anchorOriginInit]);

  useEffect(() => {
    setTransformOrigin(transformOriginInit);
  }, [transformOriginInit]);

  useEffect(() => {
    if (anchorEl && popoverEl && tailRef.current) {
      //Viewport height
      const vpHeight = Math.max(
        document.documentElement.clientHeight || 0,
        window.innerHeight || 0
      );
      //Position in the y axis of the bottom of the anchor element
      const anchorBottom =
        anchorEl.getBoundingClientRect().y +
        anchorEl.getBoundingClientRect().height;

      const paperEl = ReactDOM.findDOMNode(
        popoverEl.querySelector("#poPaperEl")
      );
      const paperHeight = (paperEl as HTMLElement).clientHeight;

      if (vpHeight - anchorBottom < paperHeight + 20) {
        //Tail should be positioned at the bottom of the popover
        tailRef.current.style.top = "unset";
        tailRef.current.style.bottom = "-7px";

        //Popover should be positioned over the anchor
        setAnchorOrigin({
          vertical: "top",
          horizontal: "center"
        });
        setTransformOrigin({
          vertical: "bottom",
          horizontal: "right"
        });
        (paperEl as HTMLElement).style.marginTop = "-8px";
      } else {
        //Tail should be positioned at the top of the popover
        tailRef.current.style.top = "-7px";
        tailRef.current.style.bottom = "unset";

        //Popover should be positioned under the anchor
        (paperEl as HTMLElement).style.marginTop = "8px";
      }
    }
  }, [anchorEl, popoverEl]);

  return {
    anchorOrigin,
    transformOrigin,
    tailRef
  };
};
