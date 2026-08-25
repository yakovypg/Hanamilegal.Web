interface ViewPanels {
  readonly root: string;
  readonly contacts: string;
  readonly error: string;
  readonly home: string;
  readonly notFound: string;
  readonly projects: string;
  readonly reviews: string;
  readonly services: string;
}

export const VIEW_PANELS: ViewPanels = {
  root: "",
  contacts: "contacts",
  error: "error",
  home: "home",
  notFound: "notfound",
  projects: "projects",
  reviews: "reviews",
  services: "services"
};

export type ViewPanel = (typeof VIEW_PANELS)[keyof typeof VIEW_PANELS];
