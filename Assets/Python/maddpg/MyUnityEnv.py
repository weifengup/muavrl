from dataclasses import dataclass
from typing import Any, Dict, List, Tuple

import gym
import gym.spaces
import numpy as np
from numpy.typing import NDArray

from peaceful_pie import unity_comms



@dataclass
class RLResult:
    observations: List[List[float]]
    rewards:List[float]
    done: List[bool]
    info: List[str]


class MyUnityEnv(gym.Env):
    def __init__(
        self,
        comms: unity_comms.UnityComms,
    ):
        self.comms = comms
        self.n=self.comms.GetAgentNum()
        self.action_space = self.comms.GetActionNum()
        obs = self.reset()
        self.observation_space = self.comms.GetActorDims()

    def reset(self):
        rl_result: RLResult = self.comms.Reset(ResultClass=RLResult)
        return self._result_to_obs(rl_result)


    def _result_to_obs(self, rl_result: RLResult):
        obs=[]
        for observation in rl_result.observations:
            obs.append(np.array(observation))
        return obs

    def step(
        self, actions
    ) :
        myactions=[]
        for action in actions:
            myactions.append(action.tolist())
        rl_result: RLResult = self.comms.Step(
            actions=myactions, ResultClass=RLResult
        )
        obs = self._result_to_obs(rl_result)
        info: Dict[str, Any] = {"finished": rl_result.done}
        return obs, rl_result.rewards, rl_result.done, info

    def close(self) -> None:
        ...