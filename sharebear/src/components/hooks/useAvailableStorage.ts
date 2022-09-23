import { useQuery } from "@tanstack/react-query";
import { useMemo } from "react";
import { GetServiceFreeSpace } from "../../../lib/fileService";

export default function useAvailableStorage() {
  return useQuery(["availableStorage"], () => GetServiceFreeSpace())
}