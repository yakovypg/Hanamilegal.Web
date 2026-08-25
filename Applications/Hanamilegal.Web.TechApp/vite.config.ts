import basicSsl from '@vitejs/plugin-basic-ssl'
import react from "@vitejs/plugin-react";
import { type ConfigEnv, defineConfig, loadEnv, type PluginOption } from "vite";

export default defineConfig(({ mode }: ConfigEnv) => {
  const env: Record<string, string> = loadEnv(mode, process.cwd(), "VITE_");

  const plugins: PluginOption[] = [react()];
  const serverPort: number = env.VITE_PORT ? Number(env.VITE_PORT) : 5173;

  if (mode !== "production") {
    plugins.push(basicSsl());
  }

  return {
    plugins: plugins,
    server: {
      host: "0.0.0.0",
      port: serverPort,
      strictPort: true
    },
    resolve: {
      tsconfigPaths: true
    }
  };
});
