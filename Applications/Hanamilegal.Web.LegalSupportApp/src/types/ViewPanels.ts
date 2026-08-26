interface ViewPanels {
  readonly root: string;
  readonly error: string;
  readonly home: string;
  readonly notFound: string;
}

export const VIEW_PANELS: ViewPanels = {
  root: "",
  error: "error",
  home: "home",
  notFound: "notfound"
};

export type ViewPanel = (typeof VIEW_PANELS)[keyof typeof VIEW_PANELS];
