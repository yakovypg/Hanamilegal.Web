import projectImage from "/example-desktop.png";

export interface ProjectConfig {
  readonly id: string | number;
  readonly translationName: string;
  readonly imageSource: string | undefined;
  readonly imageAlt?: string;
}

export const PROJECT_CONFIGS: Record<string, ProjectConfig> = {
  ypdfWeb: {
    id: "ypdfWeb",
    translationName: "ypdfWeb",
    imageSource: projectImage,
    imageAlt: "ypdfWeb"
  },
  ypdfDesktop: {
    id: "ypdfDesktop",
    translationName: "ypdfDesktop",
    imageSource: projectImage,
    imageAlt: "ypdfDesktop"
  },
  ypdfCommandLine: {
    id: "ypdfCommandLine",
    translationName: "ypdfCommandLine",
    imageSource: projectImage,
    imageAlt: "ypdfCommandLine"
  },
  netArgumentParser: {
    id: "netArgumentParser",
    translationName: "netArgumentParser",
    imageSource: projectImage,
    imageAlt: "netArgumentParser"
  }
};
