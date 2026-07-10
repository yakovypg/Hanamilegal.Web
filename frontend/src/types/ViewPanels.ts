interface ViewPanels {
  readonly root: string;
  readonly developing: string;
  readonly error: string;
  readonly home: string;
  readonly jurisprudence: string;
  readonly notFound: string;
}

export const VIEW_PANELS: ViewPanels = {
  root: "",
  developing: "developing",
  error: "error",
  home: "home",
  jurisprudence: "jurisprudence",
  notFound: "notfound"
};

export type ViewPanel = (typeof VIEW_PANELS)[keyof typeof VIEW_PANELS];
