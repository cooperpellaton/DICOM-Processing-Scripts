function lookAtBehavior(ExcelFileName)
% analysis using wheeler's Excel data files.
% Columns:
% C-F: shape info, L>0, R<0
% G-J: weights
% K: sum of weights
% N: Response code: 7=L, 2+R

excel_trials = csvread(ExcelFileName);
trialNum = size(excel_trials,1);

%%%%%%%%%%%%%%%%%%%
% psychometrics curves

% create a trial matrix for logistic curve fitting for the psychometrics
% curve
% column 1: bias term
% column 2: total weights
% column 3: results
trials = ones (trialNum, 3);
trials(:,2) = excel_trials(:,11);
trials(:,3)=excel_trials(:,12);
trials(trials(:,3)==7,3)=1;
trials(trials(:,3)==2,3)=0;
[beta, llk, pred, se] = logistfit(trials);

binnum=10; % bin trials into 10 bins by the total weights
[sortedWoe sortIndex]=sort(trials(:,2));
binsize=length(sortedWoe)/binnum;
woeAvg = zeros(binnum,1); perf = zeros(binnum,1);
t=1;
for k=1:binsize:length(sortedWoe)
    si=floor(k);
    ei=floor(k+binsize-1);
    woeAvg(t) = mean(sortedWoe(si:ei));
    perf(t) = mean(trials(sortIndex(si:ei),3))*100;
    t=t+1;
end
figure, clf, hold on;
plot(woeAvg, perf, 'bo');
woes=-4:0.1:4;
r = exp (woes*beta(2)+beta(1));
fit = r ./ (1+r) * 100;
plot(woes, fit, 'b--');
title('Pyschometrics Curve (-- logistic fitting)');
xlabel('Sum of Weights');
ylabel('% of Left Choice');

%%%%%%%%%%%%%%%%%%%
% prob-prob plot

figure, clf, hold on;
r= 10.^(woeAvg/10);
prob = r ./ (1+r);
plot(prob*100, perf, 'bo');
plot([0 100], [0 100],'b--');
title ('p-p plot');
xlabel('Prob of Left Being Correct');
ylabel('% of Left Choice');

%%%%%%%%%%%%%%%%%%%
% logistic regression
weights=[-1.17 -0.64 -0.4 0.4 0.64 1.17];
trials = zeros (trialNum, 7);

% create a trial matrix for the logistic regression
% column 1~6: the number of appearance for each shape
% column 7: bias term
% column 8: result
for i=1:trialNum
   for j=7:10
       for k=1:6
            if excel_trials(i,j)==weights(k)
                trials(i,k) = trials(i,k)+1;
                break;
            end
       end
   end
end
trials(:,7)=1;
trials(:,8)=excel_trials(:,12);
trials(trials(:,8)==7,8)=1;
trials(trials(:,8)==2,8)=0;

[beta, llk, pred, se] = logistfit(trials);
beta = beta * 10 /log(10);
figure, hold on;
plot(weights, beta(1:6),'o');
plot(0,beta(7),'x');
title('Logistic Regression for Individual Shapes');
xlabel('Weight');
ylabel('Logistic Regression Coefficients');
text(0.1, beta(7), 'bias term');

%%%%%%%%%%%%%%%%%%%
% logistic regression based on sequence

% create a trial matrix 
% column 1: bias term
% column 2-5: weights for the 4 shapes in sequence
% column 6: results
trials = ones (trialNum, 6);
trials(:,2:5) = excel_trials(:,7:10);
trials(:,6)=excel_trials(:,12);
trials(trials(:,6)==7,6)=1;
trials(trials(:,6)==2,6)=0;
[beta, llk, pred, se] = logistfit(trials);
%beta
figure
plot(0:4,beta(1:5),'o');
title('Logistic Regression Based on Sequence');
xlabel('Epoch');
ylabel('Coeff');
text(0.1,beta(1),'bias term');
