import BottomNavigationAction, {
  BottomNavigationActionProps
} from "@material-ui/core/BottomNavigationAction";
import { createStyles, Theme, withStyles } from "@material-ui/core";

/**
 *Component styles
 *
 * @remarks
 * See: https://material-ui.com/es/api/bottom-navigation-action/#bottomnavigationaction-api
 * @param theme - Material theme
 */
const styles = (theme: Theme) =>
  createStyles({
    root: (props: BottomNavigationActionProps) => {
      return {
        backgroundColor: props.selected
          ? theme.ptas.colors.utility.selection
          : theme.ptas.colors.theme.white
      };
    },
    wrapper: {
      color: theme.ptas.colors.theme.black
    }
  });

export default withStyles(styles)(BottomNavigationAction);
