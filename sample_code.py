import argparse
import os

from enum import Enum, Flag, auto

class Token(Flag):
    SPACE     = auto()
    T         = auto()
    KEY       = auto()
    RECALL    = auto()
    CREATED   = auto()
    FIXATION  = auto()
    COUNTDOWN = auto()
    NEW_TRIAL = auto()
            
def parse_line(line):
    timestr, id, message = line.split(None, 2)
    time = float(timestr)
    message = message.strip()
    if "Keypress: space" in message:
        return (Token.SPACE, time, message)
    if "Keypress: t" in message:
        return (Token.T, time, message)
    if "Keypress:" in message:
        return (Token.KEY, time, message)
    if "Recall_Phase_Instructions:" in message:
        return (Token.RECALL, time, message)
    if "Created sequence:" in message:
        return (Token.CREATED, time, message)
    if "Fixation: autoDraw = True" in message:
        return (Token.FIXATION, time, message)
    if "CountDown" in message:
        return (Token.COUNTDOWN, time, message)
    if "New trial " in message:
        return (Token.NEW_TRIAL, time, message)

class State(Enum):
    START = auto()
    TRIAL = auto()
        
class StateMachine:
    def __init__(self, transitions, start, input, output):
        self.transitions = transitions
        self.state = start
        self.input = input
        self.output = output
        self.offset = 0
        
    def run(self):
        for token, time, message in filter(None, (parse_line(line) for line in self.input)):
            for criteria, action, new_state in transitions[self.state]:
                if token & criteria:
					if action:
						action(self, token, time, message)
					self.state = new_state
                    break;
    
    def keypress(self, token, time, message):
        print(f"{time - self.offset}, {message}")
#       output.write(f"{time - self.offset}, {message}\n")
    
    def new_trial(self, token, time, message):
        # do something with the offset?
        self.offset += 1.5
        
TRANSITIONS = {
    State.START: (
        (Token.SPACE, StateMachine.new_trial, State.TRIAL),
    ),
    State.TRIAL: (
        (Token.SPACE, StateMachine.new_trial, State.TRIAL),
        (Token.KEY, StateMachine.keypress, State.TRIAL),
    ),
}
        
        
def main():
    with open('temp.log', "r") as file: 
        machine = StateMachine(TRANSITIONS, State.START, file, None)
        machine.run()

        
if __name__ == "__main__":
    main()
